using System;
using Riva.AnnouncementPreferences.Core.Enums;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.AnnouncementPreferences.Core.IntegrationEvents.UserIntegrationEvents
{
    public class UserAnnouncementPreferenceDeletedIntegrationEvent : IntegrationEventBase
    {
        public Guid UserId { get; }
        public Guid AnnouncementPreferenceId { get; }
        public AnnouncementPreferenceType AnnouncementPreferenceType { get; }

        public UserAnnouncementPreferenceDeletedIntegrationEvent(Guid correlationId, Guid userId, Guid announcementPreferenceId,
            AnnouncementPreferenceType announcementPreferenceType) : base(correlationId)
        {
            UserId = userId;
            AnnouncementPreferenceId = announcementPreferenceId;
            AnnouncementPreferenceType = announcementPreferenceType;
        }
    }
}