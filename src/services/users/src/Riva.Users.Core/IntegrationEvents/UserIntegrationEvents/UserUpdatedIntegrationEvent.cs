using System;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.Users.Core.Enums;

namespace Riva.Users.Core.IntegrationEvents.UserIntegrationEvents
{
    public class UserUpdatedIntegrationEvent : IntegrationEventBase
    {
        public Guid UserId { get; }
        public bool ServiceActive { get; }
        public AnnouncementSendingFrequency AnnouncementSendingFrequency { get; }

        public UserUpdatedIntegrationEvent(Guid correlationId, Guid userId, bool serviceActive, 
            AnnouncementSendingFrequency announcementSendingFrequency) : base(correlationId)
        {
            UserId = userId;
            ServiceActive = serviceActive;
            AnnouncementSendingFrequency = announcementSendingFrequency;
        }
    }
}