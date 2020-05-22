using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.SignalR.IntegrationEvents
{
    public class UserUpdateCompletedIntegrationEvent : IntegrationEventBase
    {
        public Guid UserId { get; }

        public UserUpdateCompletedIntegrationEvent(Guid correlationId, DateTimeOffset creationDate, 
            Guid userId) : base(correlationId, creationDate)
        {
            UserId = userId;
        }
    }
}