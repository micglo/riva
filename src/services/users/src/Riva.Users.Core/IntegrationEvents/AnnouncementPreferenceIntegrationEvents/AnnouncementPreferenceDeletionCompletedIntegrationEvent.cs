using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.Users.Core.Enums;

namespace Riva.Users.Core.IntegrationEvents.AnnouncementPreferenceIntegrationEvents
{
    public class AnnouncementPreferenceDeletionCompletedIntegrationEvent : IntegrationEventBase
    {
        public Guid UserId { get; }
        public Guid AnnouncementPreferenceId { get; }
        public AnnouncementPreferenceType AnnouncementPreferenceType { get; }

        public AnnouncementPreferenceDeletionCompletedIntegrationEvent(Guid correlationId, DateTimeOffset creationDate,
            Guid userId, Guid announcementPreferenceId, AnnouncementPreferenceType announcementPreferenceType) : base(correlationId, creationDate)
        {
            UserId = userId;
            AnnouncementPreferenceId = announcementPreferenceId;
            AnnouncementPreferenceType = announcementPreferenceType;
        }
    }
}