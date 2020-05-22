using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.SignalR.IntegrationEvents
{
    public class AccountCreationCompletedIntegrationEvent : IntegrationEventBase
    {
        public Guid AccountId { get; }

        public AccountCreationCompletedIntegrationEvent(Guid correlationId, DateTimeOffset creationDate, 
            Guid accountId) : base(correlationId, creationDate)
        {
            AccountId = accountId;
        }
    }
}