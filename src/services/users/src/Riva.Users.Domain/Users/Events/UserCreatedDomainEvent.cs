using System;
using Riva.BuildingBlocks.Domain;
using Riva.Users.Domain.Users.Enumerations;

namespace Riva.Users.Domain.Users.Events
{
    public class UserCreatedDomainEvent : DomainEventBase
    {
        public string Email { get; }
        public string Picture { get; }
        public bool ServiceActive { get; }
        public int AnnouncementPreferenceLimit { get; }
        public AnnouncementSendingFrequencyEnumeration AnnouncementSendingFrequency { get; }

        public UserCreatedDomainEvent(Guid aggregateId, Guid correlationId, string email, string picture,
            bool serviceActive, int announcementPreferenceLimit, AnnouncementSendingFrequencyEnumeration announcementSendingFrequency)
            : base(aggregateId, correlationId)
        {
            Email = email;
            Picture = picture;
            ServiceActive = serviceActive;
            AnnouncementPreferenceLimit = announcementPreferenceLimit;
            AnnouncementSendingFrequency = announcementSendingFrequency;
        }
    }
}