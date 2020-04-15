using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.Users.Core.IntegrationEvents.UserIntegrationEvents;

namespace Riva.Users.Core.IntegrationEvents.AnnouncementPreferenceIntegrationEvents.Handlers
{
    public class AnnouncementPreferenceDeletionCompletedIntegrationEventHandler : IIntegrationEventHandler<AnnouncementPreferenceDeletionCompletedIntegrationEvent>
    {
        private readonly IIntegrationEventBus _integrationEventBus;

        public AnnouncementPreferenceDeletionCompletedIntegrationEventHandler(IIntegrationEventBus integrationEventBus)
        {
            _integrationEventBus = integrationEventBus;
        }

        public Task HandleAsync(AnnouncementPreferenceDeletionCompletedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            var userAnnouncementPreferenceDeletionCompletedIntegrationEvent =
                new UserAnnouncementPreferenceDeletionCompletedIntegrationEvent(integrationEvent.CorrelationId,
                    integrationEvent.UserId, integrationEvent.AnnouncementPreferenceId, integrationEvent.AnnouncementPreferenceType);
            return _integrationEventBus.PublishIntegrationEventAsync(userAnnouncementPreferenceDeletionCompletedIntegrationEvent);
        }
    }
}