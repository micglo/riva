using System;
using Riva.BuildingBlocks.Domain;
using Riva.Users.Domain.Users.Entities;

namespace Riva.Users.Domain.Users.Events
{
    public class UserFlatForRentAnnouncementPreferenceDeletedDomainEvent : DomainEventBase
    {
        public FlatForRentAnnouncementPreference FlatForRentAnnouncementPreference { get; }

        public UserFlatForRentAnnouncementPreferenceDeletedDomainEvent(Guid aggregateId, Guid correlationId,
            FlatForRentAnnouncementPreference flatForRentAnnouncementPreference) : base(aggregateId, correlationId)
        {
            FlatForRentAnnouncementPreference = flatForRentAnnouncementPreference;
        }
    }
}