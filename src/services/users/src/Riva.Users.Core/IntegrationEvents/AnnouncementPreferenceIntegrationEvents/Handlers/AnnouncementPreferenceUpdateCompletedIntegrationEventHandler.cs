using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.Users.Core.IntegrationEvents.UserIntegrationEvents;

namespace Riva.Users.Core.IntegrationEvents.AnnouncementPreferenceIntegrationEvents.Handlers
{
    public class AnnouncementPreferenceUpdateCompletedIntegrationEventHandler : IIntegrationEventHandler<AnnouncementPreferenceUpdateCompletedIntegrationEvent>
    {
        private readonly IIntegrationEventBus _integrationEventBus;

        public AnnouncementPreferenceUpdateCompletedIntegrationEventHandler(IIntegrationEventBus integrationEventBus)
        {
            _integrationEventBus = integrationEventBus;
        }

        public Task HandleAsync(AnnouncementPreferenceUpdateCompletedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            var userAnnouncementPreferenceUpdateCompletedIntegrationEvent =
                new UserAnnouncementPreferenceUpdateCompletedIntegrationEvent(integrationEvent.CorrelationId,
                    integrationEvent.UserId, integrationEvent.AnnouncementPreferenceId,
                    integrationEvent.AnnouncementPreferenceType);
            return _integrationEventBus.PublishIntegrationEventAsync(userAnnouncementPreferenceUpdateCompletedIntegrationEvent);
        }
    }
}