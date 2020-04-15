using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.Users.Core.IntegrationEvents.UserIntegrationEvents
{
    public class UserDeletedIntegrationEvent : IntegrationEventBase
    {
        public Guid UserId { get; }

        public UserDeletedIntegrationEvent(Guid correlationId, Guid userId) : base(correlationId)
        {
            UserId = userId;
        }
    }
}