using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.Identity.Core.IntegrationEvents
{
    public class AccountCreationCompletedIntegrationEventFailure : IntegrationEventFailureBase
    {
        public Guid AccountId { get; }

        public AccountCreationCompletedIntegrationEventFailure(Guid correlationId, string code, string reason, Guid accountId) : base(correlationId, code, reason)
        {
            AccountId = accountId;
        }
    }
}