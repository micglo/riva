using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.Users.Core.IntegrationEvents.AnnouncementPreferenceIntegrationEvents
{
    public class AnnouncementPreferencesDeletionCompletedIntegrationEvent : IntegrationEventBase
    {
        public Guid UserId { get; }

        public AnnouncementPreferencesDeletionCompletedIntegrationEvent(Guid correlationId, DateTimeOffset creationDate, Guid userId) : base(correlationId, creationDate)
        {
            UserId = userId;
        }
    }
}