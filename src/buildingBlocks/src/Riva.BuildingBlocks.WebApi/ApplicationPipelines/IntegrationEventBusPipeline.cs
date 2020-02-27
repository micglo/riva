using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.Core.Communications;

namespace Riva.BuildingBlocks.WebApi.ApplicationPipelines
{
    public static class IntegrationEventBusPipeline
    {
        public static IApplicationBuilder UseIntegrationEventBus(this IApplicationBuilder builder)
        {
            var integrationEventBusSubscriptionsManager = builder.ApplicationServices.GetRequiredService<IIntegrationEventBusSubscriptionsManager>();
            integrationEventBusSubscriptionsManager.RemoveDefaultSubscriptionRuleAsync().GetAwaiter().GetResult();
            integrationEventBusSubscriptionsManager.RegisterSubscriptionMessageHandler();
            return builder;
        }
    }
}