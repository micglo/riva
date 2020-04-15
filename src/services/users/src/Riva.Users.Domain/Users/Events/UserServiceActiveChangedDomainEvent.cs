using System;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Events
{
    public class UserServiceActiveChangedDomainEvent : DomainEventBase
    {
        public bool ServiceActive { get; }

        public UserServiceActiveChangedDomainEvent(Guid aggregateId, Guid correlationId, bool serviceActive)
            : base(aggregateId, correlationId)
        {
            ServiceActive = serviceActive;
        }
    }
}