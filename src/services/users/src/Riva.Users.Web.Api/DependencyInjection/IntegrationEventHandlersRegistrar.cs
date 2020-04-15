using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.Users.Core.IntegrationEvents.AccountIntegrationEvents;
using Riva.Users.Core.IntegrationEvents.AccountIntegrationEvents.Handlers;
using Riva.Users.Core.IntegrationEvents.AnnouncementPreferenceIntegrationEvents;
using Riva.Users.Core.IntegrationEvents.AnnouncementPreferenceIntegrationEvents.Handlers;

namespace Riva.Users.Web.Api.DependencyInjection
{
    public static class IntegrationEventHandlersRegistrar
    {
        public static IServiceCollection RegisterIntegrationEventHandlers(this IServiceCollection services)
        {
            return services
                .AddTransient<IIntegrationEventHandler<AccountCreatedIntegrationEvent>, AccountCreatedIntegrationEventHandler>()
                .AddTransient<IIntegrationEventHandler<AccountDeletedIntegrationEvent>, AccountDeletedIntegrationEventHandler>()
                .AddTransient<IIntegrationEventHandler<AnnouncementPreferencesUpdateCompletedIntegrationEvent>, AnnouncementPreferencesUpdateCompletedIntegrationEventHandler>()
                .AddTransient<IIntegrationEventHandler<AnnouncementPreferencesUpdateCompletedIntegrationEventFailure>, AnnouncementPreferencesUpdateCompletedIntegrationEventFailureHandler>()
                .AddTransient<IIntegrationEventHandler<AnnouncementPreferencesDeletionCompletedIntegrationEvent>, AnnouncementPreferencesDeletionCompletedIntegrationEventHandler>()
                .AddTransient<IIntegrationEventHandler<AnnouncementPreferencesDeletionCompletedIntegrationEventFailure>, AnnouncementPreferencesDeletionCompletedIntegrationEventFailureHandler>()
                .AddTransient<IIntegrationEventHandler<AnnouncementPreferenceCreationCompletedIntegrationEvent>, AnnouncementPreferenceCreationCompletedIntegrationEventHandler>()
                .AddTransient<IIntegrationEventHandler<AnnouncementPreferenceCreationCompletedIntegrationEventFailure>, AnnouncementPreferenceCreationCompletedIntegrationEventFailureHandler>()
                .AddTransient<IIntegrationEventHandler<AnnouncementPreferenceUpdateCompletedIntegrationEvent>, AnnouncementPreferenceUpdateCompletedIntegrationEventHandler>()
                .AddTransient<IIntegrationEventHandler<AnnouncementPreferenceUpdateCompletedIntegrationEventFailure>, AnnouncementPreferenceUpdateCompletedIntegrationEventFailureHandler>()
                .AddTransient<IIntegrationEventHandler<AnnouncementPreferenceDeletionCompletedIntegrationEvent>, AnnouncementPreferenceDeletionCompletedIntegrationEventHandler>()
                .AddTransient<IIntegrationEventHandler<AnnouncementPreferenceDeletionCompletedIntegrationEventFailure>, AnnouncementPreferenceDeletionCompletedIntegrationEventFailureHandler>();
        }
    }
}