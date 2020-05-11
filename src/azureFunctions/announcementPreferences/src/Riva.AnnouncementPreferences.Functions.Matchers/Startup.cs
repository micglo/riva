using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Riva.AnnouncementPreferences.Core.Constants;
using Riva.AnnouncementPreferences.Core.Models;
using Riva.AnnouncementPreferences.Functions.Matchers;
using Riva.AnnouncementPreferences.Core.ServiceCollectionExtensions;
using Riva.AnnouncementPreferences.Core.Services;
using Riva.BuildingBlocks.Core.Logger;
using Riva.BuildingBlocks.Infrastructure.Logger;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Riva.AnnouncementPreferences.Functions.Matchers
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services
                .Configure<AppSettings>(config.GetSection("Values"))
                .AddCosmonaut(config)
                .AddLogging()
                .AddSingleton<ILogger, Logger>()
                .AddSingleton<IDocumentClient, DocumentClient>(x =>
                {
                    var connectionPolicy = new ConnectionPolicy
                    {
                        ConnectionMode = ConnectionMode.Direct,
                        ConnectionProtocol = Protocol.Tcp
                    };
                    var databaseUri = config.GetValue<string>(ConstantVariables.CosmosDbUriVariableName);
                    var databaseAuthKey = config.GetValue<string>(ConstantVariables.CosmosDbAuthKeyVariableName);
                    return new DocumentClient(new Uri(databaseUri), databaseAuthKey, connectionPolicy);
                })
                .AddSingleton<IBulkExecutorInitializer, BulkExecutorInitializer>()
                .AddSingleton<IFlatForRentAnnouncementPreferenceMatchService, FlatForRentAnnouncementPreferenceMatchService>()
                .AddSingleton<IRoomForRentAnnouncementPreferenceMatchService, RoomForRentAnnouncementPreferenceMatchService>();
        }
    }
}