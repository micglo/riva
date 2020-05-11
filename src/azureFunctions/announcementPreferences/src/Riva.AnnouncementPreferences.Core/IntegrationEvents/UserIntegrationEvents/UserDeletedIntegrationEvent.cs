using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.AnnouncementPreferences.Core.IntegrationEvents.UserIntegrationEvents
{
    public class UserDeletedIntegrationEvent : IntegrationEventBase
    {
        public Guid UserId { get; }

        public UserDeletedIntegrationEvent(Guid correlationId, DateTimeOffset creationDate, Guid userId) : base(correlationId, creationDate)
        {
            UserId = userId;
        }
    }
}