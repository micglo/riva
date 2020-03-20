using Cosmonaut;
using Cosmonaut.Extensions.Microsoft.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Entities;
using Riva.Announcements.Web.Api.Models.AppSettings;
using Riva.Announcements.Web.Api.Models.Constants;
using Riva.BuildingBlocks.WebApi.ConfigurationExtensions;

namespace Riva.Announcements.Web.Api.ServiceCollectionExtensions
{
    public static class CosmonautExtension
    {
        public static IServiceCollection AddCosmonaut(this IServiceCollection services, IConfiguration config)
        {
            var cosmosDbAppSettings = config.GetSectionAppSettings<CosmosDbAppSettings>(AppSettingsConstants.CosmosDb);
            var cosmosSettings = new CosmosStoreSettings(cosmosDbAppSettings.DatabaseName, cosmosDbAppSettings.Uri, 
                cosmosDbAppSettings.AuthKey, settings =>
                {
                    settings.JsonSerializerSettings = new JsonSerializerSettings
                    {
                        DateFormatHandling = DateFormatHandling.IsoDateFormat,
                        Formatting = Formatting.Indented,
                        Converters = { new StringEnumConverter(), new IsoDateTimeConverter() }
                    };
                });

            return services
                .AddCosmosStore<RoomForRentAnnouncementEntity>(cosmosSettings)
                .AddCosmosStore<FlatForRentAnnouncementEntity>(cosmosSettings);
        }
    }
}