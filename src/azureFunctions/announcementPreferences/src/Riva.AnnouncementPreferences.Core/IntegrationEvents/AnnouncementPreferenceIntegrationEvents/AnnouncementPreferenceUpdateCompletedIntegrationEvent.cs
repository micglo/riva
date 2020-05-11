using System;
using Riva.AnnouncementPreferences.Core.Enums;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.AnnouncementPreferences.Core.IntegrationEvents.AnnouncementPreferenceIntegrationEvents
{
    public class AnnouncementPreferenceUpdateCompletedIntegrationEvent : IntegrationEventBase
    {
        public Guid UserId { get; }
        public Guid AnnouncementPreferenceId { get; }
        public AnnouncementPreferenceType AnnouncementPreferenceType { get; }

        public AnnouncementPreferenceUpdateCompletedIntegrationEvent(Guid correlationId, Guid userId,
            Guid announcementPreferenceId, AnnouncementPreferenceType announcementPreferenceType) : base(correlationId)
        {
            UserId = userId;
            AnnouncementPreferenceId = announcementPreferenceId;
            AnnouncementPreferenceType = announcementPreferenceType;
        }
    }
}