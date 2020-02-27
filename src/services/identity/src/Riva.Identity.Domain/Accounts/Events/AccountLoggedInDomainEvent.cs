using System;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Accounts.Events
{
    public class AccountLoggedInDomainEvent : DomainEventBase
    {
        public DateTimeOffset LastLogin { get; }

        public AccountLoggedInDomainEvent(Guid aggregateId, Guid correlationId, DateTimeOffset lastLogin)
            : base(aggregateId, correlationId)
        {
            LastLogin = lastLogin;
        }
    }
}