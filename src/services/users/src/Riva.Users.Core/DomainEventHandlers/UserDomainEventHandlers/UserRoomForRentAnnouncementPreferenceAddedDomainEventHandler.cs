using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications.DomainEvents;
using Riva.BuildingBlocks.Core.Stores;
using Riva.Users.Domain.Users.Events;

namespace Riva.Users.Core.DomainEventHandlers.UserDomainEventHandlers
{
    public class UserRoomForRentAnnouncementPreferenceAddedDomainEventHandler : IDomainEventHandler<UserRoomForRentAnnouncementPreferenceAddedDomainEvent>
    {
        private readonly IDomainEventStore _domainEventStore;

        public UserRoomForRentAnnouncementPreferenceAddedDomainEventHandler(IDomainEventStore domainEventStore)
        {
            _domainEventStore = domainEventStore;
        }

        public Task HandleAsync(UserRoomForRentAnnouncementPreferenceAddedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            _domainEventStore.Store(domainEvent);
            return Task.CompletedTask;
        }
    }
}