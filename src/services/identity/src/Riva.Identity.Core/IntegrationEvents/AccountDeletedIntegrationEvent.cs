using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.Identity.Core.IntegrationEvents
{
    public class AccountDeletedIntegrationEvent : IntegrationEventBase
    {
        public Guid AccountId { get; }

        public AccountDeletedIntegrationEvent(Guid correlationId, Guid accountId) : base(correlationId)
        {
            AccountId = accountId;
        }
    }
}