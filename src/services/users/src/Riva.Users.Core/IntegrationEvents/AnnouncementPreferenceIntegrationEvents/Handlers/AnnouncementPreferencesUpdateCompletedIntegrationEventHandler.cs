using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.Users.Core.IntegrationEvents.UserIntegrationEvents;

namespace Riva.Users.Core.IntegrationEvents.AnnouncementPreferenceIntegrationEvents.Handlers
{
    public class AnnouncementPreferencesUpdateCompletedIntegrationEventHandler : IIntegrationEventHandler<AnnouncementPreferencesUpdateCompletedIntegrationEvent>
    {
        private readonly IIntegrationEventBus _integrationEventBus;

        public AnnouncementPreferencesUpdateCompletedIntegrationEventHandler(IIntegrationEventBus integrationEventBus)
        {
            _integrationEventBus = integrationEventBus;
        }

        public Task HandleAsync(AnnouncementPreferencesUpdateCompletedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            var userUpdateCompletedIntegrationEvent = new UserUpdateCompletedIntegrationEvent(integrationEvent.CorrelationId, integrationEvent.UserId);
            return _integrationEventBus.PublishIntegrationEventAsync(userUpdateCompletedIntegrationEvent);
        }
    }
}