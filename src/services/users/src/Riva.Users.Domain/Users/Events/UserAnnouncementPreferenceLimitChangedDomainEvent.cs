using System;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Events
{
    public class UserAnnouncementPreferenceLimitChangedDomainEvent : DomainEventBase
    {
        public int AnnouncementPreferenceLimit { get; }

        public UserAnnouncementPreferenceLimitChangedDomainEvent(Guid aggregateId, Guid correlationId, int announcementPreferenceLimit)
            : base(aggregateId, correlationId)
        {
            AnnouncementPreferenceLimit = announcementPreferenceLimit;
        }
    }
}