using System;
using Riva.BuildingBlocks.Domain;
using Riva.Identity.Domain.Accounts.Entities;

namespace Riva.Identity.Domain.Accounts.Events
{
    public class AccountTokenGeneratedDomainEvent : DomainEventBase
    {
        public Token Token { get; }

        public AccountTokenGeneratedDomainEvent(Guid aggregateId, Guid correlationId, Token token)
            : base(aggregateId, correlationId)
        {
            Token = token;
        }
    }
}