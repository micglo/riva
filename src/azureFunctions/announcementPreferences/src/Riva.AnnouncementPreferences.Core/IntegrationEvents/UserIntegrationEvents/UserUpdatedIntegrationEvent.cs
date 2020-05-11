using System;
using Riva.AnnouncementPreferences.Core.Enums;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.AnnouncementPreferences.Core.IntegrationEvents.UserIntegrationEvents
{
    public class UserUpdatedIntegrationEvent : IntegrationEventBase
    {
        public Guid UserId { get; }
        public bool ServiceActive { get; }
        public AnnouncementSendingFrequency AnnouncementSendingFrequency { get; }

        public UserUpdatedIntegrationEvent(Guid correlationId, DateTimeOffset creationDate, Guid userId, 
            bool serviceActive, AnnouncementSendingFrequency announcementSendingFrequency) : base(correlationId, creationDate)
        {
            UserId = userId;
            ServiceActive = serviceActive;
            AnnouncementSendingFrequency = announcementSendingFrequency;
        }
    }
}