using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.SignalR.IntegrationEvents
{
    public class AccountDeletionCompletedIntegrationEvent : IntegrationEventBase
    {
        public Guid AccountId { get; }

        public AccountDeletionCompletedIntegrationEvent(Guid correlationId, DateTimeOffset creationDate, 
            Guid accountId) : base(correlationId, creationDate)
        {
            AccountId = accountId;
        }
    }
}