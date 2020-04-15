using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.Users.Core.Enums;

namespace Riva.Users.Core.IntegrationEvents.UserIntegrationEvents
{
    public class UserAnnouncementPreferenceUpdateCompletedIntegrationEvent : IntegrationEventBase
    {
        public Guid UserId { get; }
        public Guid AnnouncementPreferenceId { get; }
        public AnnouncementPreferenceType AnnouncementPreferenceType { get; }

        public UserAnnouncementPreferenceUpdateCompletedIntegrationEvent(Guid correlationId, Guid userId, Guid announcementPreferenceId, 
            AnnouncementPreferenceType announcementPreferenceType) : base(correlationId)
        {
            UserId = userId;
            AnnouncementPreferenceId = announcementPreferenceId;
            AnnouncementPreferenceType = announcementPreferenceType;
        }
    }
}