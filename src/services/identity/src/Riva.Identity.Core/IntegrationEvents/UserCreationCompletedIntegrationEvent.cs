using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.Identity.Core.IntegrationEvents
{
    public class UserCreationCompletedIntegrationEvent : IntegrationEventBase
    {
        public Guid UserId { get; }

        public UserCreationCompletedIntegrationEvent(Guid correlationId, DateTimeOffset creationDate, Guid userId)
            : base(correlationId, creationDate)
        {
            UserId = userId;
        }
    }
}