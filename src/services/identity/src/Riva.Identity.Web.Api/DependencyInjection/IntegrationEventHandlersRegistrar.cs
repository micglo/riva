using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.Identity.Core.IntegrationEvents;
using Riva.Identity.Core.IntegrationEvents.Handlers;

namespace Riva.Identity.Web.Api.DependencyInjection
{
    public static class IntegrationEventHandlersRegistrar
    {
        public static IServiceCollection RegisterIntegrationEventHandlers(this IServiceCollection services)
        {
            return services
                .AddTransient<IIntegrationEventHandler<UserCreationCompletedIntegrationEvent>, UserCreationCompletedIntegrationEventHandler>()
                .AddTransient<IIntegrationEventHandler<UserCreationCompletedIntegrationEventFailure>, UserCreationCompletedIntegrationEventFailureHandler>()
                .AddTransient<IIntegrationEventHandler<UserDeletionCompletedIntegrationEvent>, UserDeletionCompletedIntegrationEventHandler>()
                .AddTransient<IIntegrationEventHandler<UserDeletionCompletedIntegrationEventFailure>, UserDeletionCompletedIntegrationEventFailureHandler>();
        }
    }
}