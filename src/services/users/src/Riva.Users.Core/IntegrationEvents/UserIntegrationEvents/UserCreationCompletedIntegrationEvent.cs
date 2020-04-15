using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.Users.Core.IntegrationEvents.UserIntegrationEvents
{
    public class UserCreationCompletedIntegrationEvent : IntegrationEventBase
    {
        public Guid UserId { get; }

        public UserCreationCompletedIntegrationEvent(Guid correlationId, Guid userId) : base(correlationId)
        {
            UserId = userId;
        }
    }
}