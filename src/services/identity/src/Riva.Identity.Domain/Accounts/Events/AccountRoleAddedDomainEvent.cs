using System;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Accounts.Events
{
    public class AccountRoleAddedDomainEvent : DomainEventBase
    {
        public Guid Role { get; }

        public AccountRoleAddedDomainEvent(Guid aggregateId, Guid correlationId, Guid role)
            : base(aggregateId, correlationId)
        {
            Role = role;
        }
    }
}