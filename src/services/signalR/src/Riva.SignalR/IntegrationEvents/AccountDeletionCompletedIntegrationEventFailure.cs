using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.SignalR.IntegrationEvents
{
    public class AccountDeletionCompletedIntegrationEventFailure : IntegrationEventFailureBase
    {
        public Guid AccountId { get; }

        public AccountDeletionCompletedIntegrationEventFailure(Guid correlationId, DateTimeOffset creationDate, 
            string code, string reason, Guid accountId) : base(correlationId, creationDate, code, reason)
        {
            AccountId = accountId;
        }
    }
}