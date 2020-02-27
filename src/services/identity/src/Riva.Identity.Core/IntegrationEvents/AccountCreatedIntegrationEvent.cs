using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.Identity.Core.IntegrationEvents
{
    public class AccountCreatedIntegrationEvent : IntegrationEventBase
    {
        public Guid AccountId { get; }
        public string Email { get; }
        public string Picture { get; }

        public AccountCreatedIntegrationEvent(Guid correlationId, Guid accountId, string email, string picture) : base(correlationId)
        {
            AccountId = accountId;
            Email = email;
            Picture = picture;
        }
    }
}