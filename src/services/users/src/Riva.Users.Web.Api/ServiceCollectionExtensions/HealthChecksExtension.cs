using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.WebApi.ConfigurationExtensions;
using Riva.BuildingBlocks.WebApi.Models.AppSettings;
using Riva.BuildingBlocks.WebApi.Models.Constants;

namespace Riva.Users.Web.Api.ServiceCollectionExtensions
{
    public static class HealthChecksExtension
    {
        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration config)
        {
            var connectionStringAppSettings = config.GetSectionAppSettings<Models.AppSettings.ConnectionStringsAppSettings>(AppSettingsConstants.ConnectionStrings);
            var centralServiceBusAppSettings = config.GetSectionAppSettings<CentralServiceBusAppSettings>(AppSettingsConstants.CentralServiceBus);

            return BuildingBlocks.WebApi.ServiceCollectionExtensions.HealthChecksExtension.AddHealthChecks(services)
                .AddSqlServer(connectionStringAppSettings.RivaUsersSQLServerDatabaseConnectionString, name: "RivaUsersSQLServerDatabase")
                .AddAzureServiceBusTopic(connectionStringAppSettings.CentralServiceBusConnectionString, centralServiceBusAppSettings.TopicName, name: "CentralServiceBus")
                .Services;
        }
    }
}