using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.WebApi.ConfigurationExtensions;
using Riva.BuildingBlocks.WebApi.Models.AppSettings;
using Riva.BuildingBlocks.WebApi.Models.Constants;

namespace Riva.SignalR.ServiceCollectionExtensions
{
    public static class HealthChecksExtension
    {
        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration config)
        {
            var connectionStringAppSettings = config.GetSectionAppSettings<ConnectionStringsAppSettings>(AppSettingsConstants.ConnectionStrings);
            var centralServiceBusAppSettings = config.GetSectionAppSettings<CentralServiceBusAppSettings>(AppSettingsConstants.CentralServiceBus);

            return BuildingBlocks.WebApi.ServiceCollectionExtensions.HealthChecksExtension.AddHealthChecks(services)
                .AddAzureServiceBusTopic(connectionStringAppSettings.CentralServiceBusConnectionString, centralServiceBusAppSettings.TopicName, name: "CentralServiceBus")
                .Services;
        }
    }
}