using System;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Accounts.Events
{
    public class AccountPasswordChangedDomainEvent : DomainEventBase
    {
        public string PasswordHash { get; }

        public AccountPasswordChangedDomainEvent(Guid aggregateId, Guid correlationId, string passwordHash)
            : base(aggregateId, correlationId)
        {
            PasswordHash = passwordHash;
        }
    }
}