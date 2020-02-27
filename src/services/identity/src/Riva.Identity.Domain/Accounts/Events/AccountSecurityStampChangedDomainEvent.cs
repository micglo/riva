using System;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Accounts.Events
{
    public class AccountSecurityStampChangedDomainEvent : DomainEventBase
    {
        public Guid SecurityStamp { get; }

        public AccountSecurityStampChangedDomainEvent(Guid aggregateId, Guid correlationId, Guid securityStamp)
            : base(aggregateId, correlationId)
        {
            SecurityStamp = securityStamp;
        }
    }
}