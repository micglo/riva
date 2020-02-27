using System;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Accounts.Events
{
    public class AccountConfirmedDomainEvent : DomainEventBase
    {
        public AccountConfirmedDomainEvent(Guid aggregateId, Guid correlationId) : base(aggregateId, correlationId)
        {
        }
    }
}