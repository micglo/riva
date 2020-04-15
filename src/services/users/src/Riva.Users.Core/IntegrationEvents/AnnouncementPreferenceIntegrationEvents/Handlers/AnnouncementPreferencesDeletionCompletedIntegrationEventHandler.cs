using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.Users.Core.IntegrationEvents.UserIntegrationEvents;

namespace Riva.Users.Core.IntegrationEvents.AnnouncementPreferenceIntegrationEvents.Handlers
{
    public class AnnouncementPreferencesDeletionCompletedIntegrationEventHandler : IIntegrationEventHandler<AnnouncementPreferencesDeletionCompletedIntegrationEvent>
    {
        private readonly IIntegrationEventBus _integrationEventBus;

        public AnnouncementPreferencesDeletionCompletedIntegrationEventHandler(IIntegrationEventBus integrationEventBus)
        {
            _integrationEventBus = integrationEventBus;
        }

        public Task HandleAsync(AnnouncementPreferencesDeletionCompletedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            var userDeletionCompletedIntegrationEvent = new UserDeletionCompletedIntegrationEvent(integrationEvent.CorrelationId, integrationEvent.UserId);
            return _integrationEventBus.PublishIntegrationEventAsync(userDeletionCompletedIntegrationEvent);
        }
    }
}