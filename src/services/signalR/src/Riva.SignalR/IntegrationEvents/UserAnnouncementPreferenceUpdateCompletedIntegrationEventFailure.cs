using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.SignalR.Enums;

namespace Riva.SignalR.IntegrationEvents
{
    public class UserAnnouncementPreferenceUpdateCompletedIntegrationEventFailure : IntegrationEventFailureBase
    {
        public Guid UserId { get; }
        public Guid AnnouncementPreferenceId { get; }
        public AnnouncementPreferenceType AnnouncementPreferenceType { get; }

        public UserAnnouncementPreferenceUpdateCompletedIntegrationEventFailure(Guid correlationId, DateTimeOffset creationDate, 
            string code, string reason, Guid userId, Guid announcementPreferenceId, AnnouncementPreferenceType announcementPreferenceType)
            : base(correlationId, creationDate, code, reason)
        {
            UserId = userId;
            AnnouncementPreferenceId = announcementPreferenceId;
            AnnouncementPreferenceType = announcementPreferenceType;
        }
    }
}