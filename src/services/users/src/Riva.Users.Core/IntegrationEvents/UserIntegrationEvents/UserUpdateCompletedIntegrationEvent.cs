using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.Users.Core.IntegrationEvents.UserIntegrationEvents
{
    public class UserUpdateCompletedIntegrationEvent : IntegrationEventBase
    {
        public Guid UserId { get; }

        public UserUpdateCompletedIntegrationEvent(Guid correlationId, Guid userId) : base(correlationId)
        {
            UserId = userId;
        }
    }
}