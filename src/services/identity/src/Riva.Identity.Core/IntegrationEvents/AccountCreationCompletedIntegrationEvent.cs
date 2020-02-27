using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.Identity.Core.IntegrationEvents
{
    public class AccountCreationCompletedIntegrationEvent : IntegrationEventBase
    {
        public Guid AccountId { get; }

        public AccountCreationCompletedIntegrationEvent(Guid correlationId, Guid accountId) : base(correlationId)
        {
            AccountId = accountId;
        }
    }
}