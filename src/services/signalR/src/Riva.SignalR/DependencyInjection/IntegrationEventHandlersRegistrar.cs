using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.SignalR.IntegrationEvents;
using Riva.SignalR.IntegrationEvents.Handlers;

namespace Riva.SignalR.DependencyInjection
{
    public static class IntegrationEventHandlersRegistrar
    {
        public static IServiceCollection RegisterIntegrationEventHandlers(this IServiceCollection services)
        {
            return services
                .AddTransient<IIntegrationEventHandler<AccountCreationCompletedIntegrationEvent>, AccountCreationCompletedIntegrationEventHandler>()
                .AddTransient<IIntegrationEventHandler<AccountCreationCompletedIntegrationEventFailure>, AccountCreationCompletedIntegrationEventFailureHandler>()
                .AddTransient<IIntegrationEventHandler<AccountDeletionCompletedIntegrationEvent>, AccountDeletionCompletedIntegrationEventHandler>()
                .AddTransient<IIntegrationEventHandler<AccountDeletionCompletedIntegrationEventFailure>, AccountDeletionCompletedIntegrationEventFailureHandler>()
                .AddTransient<IIntegrationEventHandler<UserAnnouncementPreferenceCreationCompletedIntegrationEvent>, UserAnnouncementPreferenceCreationCompletedIntegrationEventHandler>()
                .AddTransient<IIntegrationEventHandler<UserAnnouncementPreferenceCreationCompletedIntegrationEventFailure>, UserAnnouncementPreferenceCreationCompletedIntegrationEventFailureHandler>()
                .AddTransient<IIntegrationEventHandler<UserAnnouncementPreferenceDeletionCompletedIntegrationEvent>, UserAnnouncementPreferenceDeletionCompletedIntegrationEventHandler>()
                .AddTransient<IIntegrationEventHandler<UserAnnouncementPreferenceDeletionCompletedIntegrationEventFailure>, UserAnnouncementPreferenceDeletionCompletedIntegrationEventFailureHandler>()
                .AddTransient<IIntegrationEventHandler<UserAnnouncementPreferenceUpdateCompletedIntegrationEvent>, UserAnnouncementPreferenceUpdateCompletedIntegrationEventHandler>()
                .AddTransient<IIntegrationEventHandler<UserAnnouncementPreferenceUpdateCompletedIntegrationEventFailure>, UserAnnouncementPreferenceUpdateCompletedIntegrationEventFailureHandler>()
                .AddTransient<IIntegrationEventHandler<UserUpdateCompletedIntegrationEvent>, UserUpdateCompletedIntegrationEventHandler>()
                .AddTransient<IIntegrationEventHandler<UserUpdateCompletedIntegrationEventFailure>, UserUpdateCompletedIntegrationEventFailureHandler>();
        }
    }
}