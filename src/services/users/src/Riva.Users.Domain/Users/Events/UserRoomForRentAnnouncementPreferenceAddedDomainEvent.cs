using System;
using Riva.BuildingBlocks.Domain;
using Riva.Users.Domain.Users.Entities;

namespace Riva.Users.Domain.Users.Events
{
    public class UserRoomForRentAnnouncementPreferenceAddedDomainEvent : DomainEventBase
    {
        public RoomForRentAnnouncementPreference RoomForRentAnnouncementPreference { get; }

        public UserRoomForRentAnnouncementPreferenceAddedDomainEvent(Guid aggregateId, Guid correlationId,
            RoomForRentAnnouncementPreference roomForRentAnnouncementPreference) : base(aggregateId, correlationId)
        {
            RoomForRentAnnouncementPreference = roomForRentAnnouncementPreference;
        }
    }
}