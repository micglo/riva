using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.SignalR.IntegrationEvents
{
    public class AccountCreationCompletedIntegrationEventFailure : IntegrationEventFailureBase
    {
        public Guid AccountId { get; }

        public AccountCreationCompletedIntegrationEventFailure(Guid correlationId, DateTimeOffset creationDate, 
            string code, string reason, Guid accountId) : base(correlationId, creationDate, code, reason)
        {
            AccountId = accountId;
        }
    }
}