using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.Users.Core.IntegrationEvents.UserIntegrationEvents;

namespace Riva.Users.Core.IntegrationEvents.AnnouncementPreferenceIntegrationEvents.Handlers
{
    public class AnnouncementPreferenceCreationCompletedIntegrationEventHandler : IIntegrationEventHandler<AnnouncementPreferenceCreationCompletedIntegrationEvent>
    {
        private readonly IIntegrationEventBus _integrationEventBus;

        public AnnouncementPreferenceCreationCompletedIntegrationEventHandler(IIntegrationEventBus integrationEventBus)
        {
            _integrationEventBus = integrationEventBus;
        }

        public Task HandleAsync(AnnouncementPreferenceCreationCompletedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            var userAnnouncementPreferenceCreationCompletedIntegrationEvent =
                new UserAnnouncementPreferenceCreationCompletedIntegrationEvent(integrationEvent.CorrelationId,
                    integrationEvent.UserId, integrationEvent.AnnouncementPreferenceId,
                    integrationEvent.AnnouncementPreferenceType);
            return _integrationEventBus.PublishIntegrationEventAsync(userAnnouncementPreferenceCreationCompletedIntegrationEvent);
        }
    }
}