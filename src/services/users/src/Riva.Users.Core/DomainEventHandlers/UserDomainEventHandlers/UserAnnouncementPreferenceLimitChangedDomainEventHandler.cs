﻿using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications.DomainEvents;
using Riva.BuildingBlocks.Core.Stores;
using Riva.Users.Domain.Users.Events;

namespace Riva.Users.Core.DomainEventHandlers.UserDomainEventHandlers
{
    public class UserAnnouncementPreferenceLimitChangedDomainEventHandler : IDomainEventHandler<UserAnnouncementPreferenceLimitChangedDomainEvent>
    {
        private readonly IDomainEventStore _domainEventStore;

        public UserAnnouncementPreferenceLimitChangedDomainEventHandler(IDomainEventStore domainEventStore)
        {
            _domainEventStore = domainEventStore;
        }

        public Task HandleAsync(UserAnnouncementPreferenceLimitChangedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            _domainEventStore.Store(domainEvent);
            return Task.CompletedTask;
        }
    }
}