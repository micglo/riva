using System;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Accounts.Events
{
    public class AccountDeletedDomainEvent : DomainEventBase
    {
        public AccountDeletedDomainEvent(Guid aggregateId, Guid correlationId) : base(aggregateId, correlationId)
        {
        }
    }
}