using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.Identity.Core.IntegrationEvents
{
    public class UserDeletionCompletedIntegrationEvent : IntegrationEventBase
    {
        public Guid UserId { get; }

        public UserDeletionCompletedIntegrationEvent(Guid correlationId, DateTimeOffset creationDate, Guid userId)
            : base(correlationId, creationDate)
        {
            UserId = userId;
        }
    }
}