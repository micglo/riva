using System;
using System.Collections.Generic;
using System.Linq;
using Riva.AnnouncementPreferences.Core.Models;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.AnnouncementPreferences.Core.IntegrationEvents.AnnouncementIntegrationEvents
{
    public class RoomForRentAnnouncementsIntegrationEvent : IntegrationEventBase
    {
        public Guid CityId { get; }
        public IReadOnlyCollection<RoomForRentAnnouncement> RoomForRentAnnouncements { get; }

        public RoomForRentAnnouncementsIntegrationEvent(Guid correlationId, DateTimeOffset creationDate,
            Guid cityId, IEnumerable<RoomForRentAnnouncement> roomForRentAnnouncements) : base(correlationId, creationDate)
        {
            CityId = cityId;
            RoomForRentAnnouncements = roomForRentAnnouncements.ToList().AsReadOnly();
        }
    }
}