using System;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Accounts.Events
{
    public class AccountRoleDeletedDomainEvent : DomainEventBase
    {
        public Guid Role { get; }

        public AccountRoleDeletedDomainEvent(Guid aggregateId, Guid correlationId, Guid role)
            : base(aggregateId, correlationId)
        {
            Role = role;
        }
    }
}