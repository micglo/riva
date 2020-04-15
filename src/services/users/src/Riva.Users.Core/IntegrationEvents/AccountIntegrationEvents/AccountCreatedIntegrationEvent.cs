using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.Users.Core.IntegrationEvents.AccountIntegrationEvents
{
    public class AccountCreatedIntegrationEvent : IntegrationEventBase
    {
        public Guid AccountId { get; }
        public string Email { get; }
        public string Picture { get; }

        public AccountCreatedIntegrationEvent(Guid correlationId, DateTimeOffset creationDate, 
            Guid accountId, string email, string picture) : base(correlationId, creationDate)
        {
            AccountId = accountId;
            Email = email;
            Picture = picture;
        }
    }
}