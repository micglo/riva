using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Cosmonaut.Extensions;
using Microsoft.Azure.CosmosDB.BulkExecutor;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;
using Riva.AnnouncementPreferences.Core.Constants;
using Riva.AnnouncementPreferences.Core.Models;

namespace Riva.AnnouncementPreferences.Core.Services
{
    public class BulkExecutorInitializer : IBulkExecutorInitializer
    {
        private readonly string _databaseName;
        private readonly int _collectionThroughput;
        private readonly DocumentClient _client;

        public BulkExecutorInitializer(IDocumentClient client, IOptions<AppSettings> options)
        {
            _databaseName = options.Value.CosmosDbDatabaseName;
            _collectionThroughput = Convert.ToInt32(options.Value.CosmosDbCollectionThroughput);
            _client = new DocumentClient(client.ServiceEndpoint, client.AuthKey, client.ConnectionPolicy);
        }

        public async Task<IBulkExecutor> InitializeBulkExecutorAsync()
        {
            var dataCollection = await GetDocumentCollectionAsync(_databaseName, ConstantVariables.CosmosCollectionName, _collectionThroughput,
                ConstantVariables.CosmosDbCollectionPartitionKey);

            _client.ConnectionPolicy.RetryOptions.MaxRetryWaitTimeInSeconds = 30;
            _client.ConnectionPolicy.RetryOptions.MaxRetryAttemptsOnThrottledRequests = 9;

            var bulkExecutor = new BulkExecutor(_client, dataCollection);
            await bulkExecutor.InitializeAsync();

            _client.ConnectionPolicy.RetryOptions.MaxRetryWaitTimeInSeconds = 0;
            _client.ConnectionPolicy.RetryOptions.MaxRetryAttemptsOnThrottledRequests = 0;

            return bulkExecutor;
        }

        private async Task<DocumentCollection> GetDocumentCollectionAsync(string databaseName, string collectionName,
            int collectionThroughput, string collectionPartitionKey)
        {
            var database = await GetDatabaseAsync(databaseName);
            return await CreatePartitionedCollectionAsync(database.SelfLink, collectionName, collectionThroughput,
                collectionPartitionKey);
        }

        private async Task<Database> GetDatabaseAsync(string databaseName)
        {
            return await _client.CreateDatabaseQuery().FirstOrDefaultAsync(x => x.Id == databaseName) ??
                   await _client.CreateDatabaseAsync(new Database {Id = databaseName});
        }

        private async Task<DocumentCollection> CreatePartitionedCollectionAsync(string databaseLink,
            string collectionName, int collectionThroughput, string collectionPartitionKey)
        {
            var collection = await _client.CreateDocumentCollectionQuery(databaseLink).FirstOrDefaultAsync(x => x.Id.Equals(collectionName));
            if (collection != null)
                return collection;

            var partitionKey = new PartitionKeyDefinition
            {
                Paths = new Collection<string> { collectionPartitionKey }
            };
            collection = new DocumentCollection { Id = collectionName, PartitionKey = partitionKey };

            return await _client.CreateDocumentCollectionAsync(databaseLink, collection,
                new RequestOptions { OfferThroughput = collectionThroughput });
        }
    }
}