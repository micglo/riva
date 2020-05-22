using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.SignalR.Enums;

namespace Riva.SignalR.IntegrationEvents
{
    public class UserAnnouncementPreferenceDeletionCompletedIntegrationEvent : IntegrationEventBase
    {
        public Guid UserId { get; }
        public Guid AnnouncementPreferenceId { get; }
        public AnnouncementPreferenceType AnnouncementPreferenceType { get; }

        public UserAnnouncementPreferenceDeletionCompletedIntegrationEvent(Guid correlationId, DateTimeOffset creationDate, 
            Guid userId, Guid announcementPreferenceId, AnnouncementPreferenceType announcementPreferenceType) : base(correlationId, creationDate)
        {
            UserId = userId;
            AnnouncementPreferenceId = announcementPreferenceId;
            AnnouncementPreferenceType = announcementPreferenceType;
        }
    }
}