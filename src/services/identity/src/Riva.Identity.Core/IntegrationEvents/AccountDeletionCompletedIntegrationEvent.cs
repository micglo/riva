using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.Identity.Core.IntegrationEvents
{
    public class AccountDeletionCompletedIntegrationEvent : IntegrationEventBase
    {
        public Guid AccountId { get; }

        public AccountDeletionCompletedIntegrationEvent(Guid correlationId, Guid accountId) : base(correlationId)
        {
            AccountId = accountId;
        }
    }
}