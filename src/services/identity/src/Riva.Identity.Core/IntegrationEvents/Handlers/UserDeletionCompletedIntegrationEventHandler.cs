using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.Identity.Core.IntegrationEvents.Handlers
{
    public class UserDeletionCompletedIntegrationEventHandler : IIntegrationEventHandler<UserDeletionCompletedIntegrationEvent>
    {
        private readonly IIntegrationEventBus _integrationEventBus;

        public UserDeletionCompletedIntegrationEventHandler(IIntegrationEventBus integrationEventBus)
        {
            _integrationEventBus = integrationEventBus;
        }

        public Task HandleAsync(UserDeletionCompletedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            var accountDeletionCompletedIntegrationEvent =
                new AccountDeletionCompletedIntegrationEvent(integrationEvent.CorrelationId, integrationEvent.UserId);
            return _integrationEventBus.PublishIntegrationEventAsync(accountDeletionCompletedIntegrationEvent);
        }
    }
}