using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.SignalR.Enums;

namespace Riva.SignalR.IntegrationEvents
{
    public class UserAnnouncementPreferenceCreationCompletedIntegrationEvent : IntegrationEventBase
    {
        public Guid UserId { get; }
        public Guid AnnouncementPreferenceId { get; }
        public AnnouncementPreferenceType AnnouncementPreferenceType { get; }

        public UserAnnouncementPreferenceCreationCompletedIntegrationEvent(Guid correlationId, DateTimeOffset creationDate, 
            Guid userId, Guid announcementPreferenceId, AnnouncementPreferenceType announcementPreferenceType) : base(correlationId, creationDate)
        {
            UserId = userId;
            AnnouncementPreferenceId = announcementPreferenceId;
            AnnouncementPreferenceType = announcementPreferenceType;
        }
    }
}