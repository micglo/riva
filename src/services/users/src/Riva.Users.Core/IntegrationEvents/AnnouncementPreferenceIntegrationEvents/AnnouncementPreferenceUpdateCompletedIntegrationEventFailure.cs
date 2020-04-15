using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.Users.Core.Enums;

namespace Riva.Users.Core.IntegrationEvents.AnnouncementPreferenceIntegrationEvents
{
    public class AnnouncementPreferenceUpdateCompletedIntegrationEventFailure : IntegrationEventFailureBase
    {
        public Guid UserId { get; }
        public Guid AnnouncementPreferenceId { get; }
        public AnnouncementPreferenceType AnnouncementPreferenceType { get; }

        public AnnouncementPreferenceUpdateCompletedIntegrationEventFailure(Guid correlationId, DateTimeOffset creationDate,
            string code, string reason, Guid userId, Guid announcementPreferenceId, AnnouncementPreferenceType announcementPreferenceType)
            : base(correlationId, creationDate, code, reason)
        {
            UserId = userId;
            AnnouncementPreferenceId = announcementPreferenceId;
            AnnouncementPreferenceType = announcementPreferenceType;
        }
    }
}