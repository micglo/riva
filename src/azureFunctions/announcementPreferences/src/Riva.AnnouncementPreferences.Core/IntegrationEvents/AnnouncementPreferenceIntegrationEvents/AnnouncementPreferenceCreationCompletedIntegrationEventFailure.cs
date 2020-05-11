using System;
using Riva.AnnouncementPreferences.Core.Enums;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.AnnouncementPreferences.Core.IntegrationEvents.AnnouncementPreferenceIntegrationEvents
{
    public class AnnouncementPreferenceCreationCompletedIntegrationEventFailure : IntegrationEventFailureBase
    {
        public Guid UserId { get; }
        public Guid AnnouncementPreferenceId { get; }
        public AnnouncementPreferenceType AnnouncementPreferenceType { get; }

        public AnnouncementPreferenceCreationCompletedIntegrationEventFailure(Guid correlationId, string code, string reason, 
            Guid userId, Guid announcementPreferenceId, AnnouncementPreferenceType announcementPreferenceType) : base(correlationId, code, reason)
        {
            UserId = userId;
            AnnouncementPreferenceId = announcementPreferenceId;
            AnnouncementPreferenceType = announcementPreferenceType;
        }
    }
}