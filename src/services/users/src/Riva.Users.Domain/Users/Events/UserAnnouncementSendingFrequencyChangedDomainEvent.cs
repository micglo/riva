using System;
using Riva.BuildingBlocks.Domain;
using Riva.Users.Domain.Users.Enumerations;

namespace Riva.Users.Domain.Users.Events
{
    public class UserAnnouncementSendingFrequencyChangedDomainEvent : DomainEventBase
    {
        public AnnouncementSendingFrequencyEnumeration AnnouncementSendingFrequency { get; }

        public UserAnnouncementSendingFrequencyChangedDomainEvent(Guid aggregateId, Guid correlationId, 
            AnnouncementSendingFrequencyEnumeration announcementSendingFrequency) : base(aggregateId, correlationId)
        {
            AnnouncementSendingFrequency = announcementSendingFrequency;
        }
    }
}