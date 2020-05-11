using Cosmonaut;
using Cosmonaut.Extensions.Microsoft.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Riva.AnnouncementPreferences.Core.Constants;
using Riva.AnnouncementPreferences.Core.Entities;

namespace Riva.AnnouncementPreferences.Core.ServiceCollectionExtensions
{
    public static class CosmonautExtension
    {
        public static IServiceCollection AddCosmonaut(this IServiceCollection services, IConfiguration config)
        {
            var databaseName = config.GetValue<string>(ConstantVariables.CosmosDbDatabaseNameVariableName);
            var databaseUri = config.GetValue<string>(ConstantVariables.CosmosDbUriVariableName);
            var databaseAuthKey = config.GetValue<string>(ConstantVariables.CosmosDbAuthKeyVariableName);
            var collectionThroughput = config.GetValue<int>(ConstantVariables.CosmosDbCollectionThroughputVariableName);
            var cosmosSettings = new CosmosStoreSettings(databaseName, databaseUri, databaseAuthKey, settings =>
            {
                settings.DefaultCollectionThroughput = collectionThroughput;
                settings.JsonSerializerSettings = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    Formatting = Formatting.Indented,
                    Converters = { new StringEnumConverter(), new IsoDateTimeConverter() }
                };
            });

            return services
                .AddCosmosStore<FlatForRentAnnouncementPreference>(cosmosSettings)
                .AddCosmosStore<RoomForRentAnnouncementPreference>(cosmosSettings);
        }
    }
}