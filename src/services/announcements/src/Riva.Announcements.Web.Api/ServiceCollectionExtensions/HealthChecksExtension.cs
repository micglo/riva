using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Riva.Announcements.Web.Api.Models.AppSettings;
using Riva.BuildingBlocks.WebApi.ConfigurationExtensions;
using Riva.BuildingBlocks.WebApi.Models.Constants;

namespace Riva.Announcements.Web.Api.ServiceCollectionExtensions
{
    public static class HealthChecksExtension
    {
        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration config)
        {
            var connectionStringAppSettings = config.GetSectionAppSettings<ConnectionStringsAppSettings>(AppSettingsConstants.ConnectionStrings);

            return BuildingBlocks.WebApi.ServiceCollectionExtensions.HealthChecksExtension.AddHealthChecks(services)
                .AddCosmosDb(connectionStringAppSettings.RivaAnnouncementsCosmosDbConnectionString, name: "RivaAnnouncementsCosmosDb")
                .Services;
        }
    }
}