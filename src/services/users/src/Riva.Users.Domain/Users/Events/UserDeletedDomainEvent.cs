using System;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Events
{
    public class UserDeletedDomainEvent : DomainEventBase
    {
        public UserDeletedDomainEvent(Guid aggregateId, Guid correlationId)
            : base(aggregateId, correlationId)
        {
        }
    }
}