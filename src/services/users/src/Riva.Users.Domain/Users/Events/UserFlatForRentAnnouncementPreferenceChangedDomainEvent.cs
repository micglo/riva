using System;
using Riva.BuildingBlocks.Domain;
using Riva.Users.Domain.Users.Entities;

namespace Riva.Users.Domain.Users.Events
{
    public class UserFlatForRentAnnouncementPreferenceChangedDomainEvent : DomainEventBase
    {
        public FlatForRentAnnouncementPreference FlatForRentAnnouncementPreference { get; }

        public UserFlatForRentAnnouncementPreferenceChangedDomainEvent(Guid aggregateId, Guid correlationId,
            FlatForRentAnnouncementPreference flatForRentAnnouncementPreference) : base(aggregateId, correlationId)
        {
            FlatForRentAnnouncementPreference = flatForRentAnnouncementPreference;
        }
    }
}