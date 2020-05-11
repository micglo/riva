using System;
using System.Collections.Generic;
using System.Linq;
using Riva.AnnouncementPreferences.Core.Models;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.AnnouncementPreferences.Core.IntegrationEvents.AnnouncementIntegrationEvents
{
    public class FlatForRentAnnouncementsIntegrationEvent : IntegrationEventBase
    {
        public Guid CityId { get; }
        public IReadOnlyCollection<FlatForRentAnnouncement> FlatForRentAnnouncements { get; }

        public FlatForRentAnnouncementsIntegrationEvent(Guid correlationId, DateTimeOffset creationDate,
            Guid cityId, IEnumerable<FlatForRentAnnouncement> flatForRentAnnouncements) : base(correlationId, creationDate)
        {
            CityId = cityId;
            FlatForRentAnnouncements = flatForRentAnnouncements.ToList().AsReadOnly();
        }
    }
}