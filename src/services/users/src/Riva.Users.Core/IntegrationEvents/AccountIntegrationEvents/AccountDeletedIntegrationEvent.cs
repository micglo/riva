using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.Users.Core.IntegrationEvents.AccountIntegrationEvents
{
    public class AccountDeletedIntegrationEvent : IntegrationEventBase
    {
        public Guid AccountId { get; }

        public AccountDeletedIntegrationEvent(Guid correlationId, DateTimeOffset creationDate, Guid accountId) : base(correlationId, creationDate)
        {
            AccountId = accountId;
        }
    }
}