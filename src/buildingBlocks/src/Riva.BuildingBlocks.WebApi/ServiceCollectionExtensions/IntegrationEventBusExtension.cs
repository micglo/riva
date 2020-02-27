using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Logger;
using Riva.BuildingBlocks.Infrastructure.Communications;

namespace Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions
{
    public static class IntegrationEventBusExtension
    {
        public static IServiceCollection AddIntegrationEventBus(this IServiceCollection services, string centralServiceBusConnectionString)
        {
            return services
                .AddSingleton<IServiceBusPersisterConnection>(sp =>
                    new ServiceBusPersisterConnection(
                        new ServiceBusConnectionStringBuilder(centralServiceBusConnectionString)))
                .AddSingleton<IIntegrationEventBus, IntegrationEventBus>();
        }

        public static IServiceCollection AddIntegrationEventBus(this IServiceCollection services, string centralServiceBusConnectionString, string subscriptionName)
        {
            return services
                .AddSingleton<IServiceBusPersisterConnection>(sp =>
                    new ServiceBusPersisterConnection(
                        new ServiceBusConnectionStringBuilder(centralServiceBusConnectionString)))
                .AddSingleton<IIntegrationEventBusSubscriptionsManager, IntegrationEventBusSubscriptionsManager>(sp =>
                {
                    var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersisterConnection>();
                    var logger = sp.GetRequiredService<ILogger>();
                    return new IntegrationEventBusSubscriptionsManager(serviceBusPersisterConnection, sp, logger, subscriptionName);
                })
                .AddSingleton<IIntegrationEventBus, IntegrationEventBus>();
        }
    }
}