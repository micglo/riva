using System;
using Riva.BuildingBlocks.Domain;
using Riva.Users.Domain.Users.Entities;

namespace Riva.Users.Domain.Users.Events
{
    public class UserRoomForRentAnnouncementPreferenceDeletedDomainEvent : DomainEventBase
    {
        public RoomForRentAnnouncementPreference RoomForRentAnnouncementPreference { get; }

        public UserRoomForRentAnnouncementPreferenceDeletedDomainEvent(Guid aggregateId, Guid correlationId,
            RoomForRentAnnouncementPreference roomForRentAnnouncementPreference) : base(aggregateId, correlationId)
        {
            RoomForRentAnnouncementPreference = roomForRentAnnouncementPreference;
        }
    }
}