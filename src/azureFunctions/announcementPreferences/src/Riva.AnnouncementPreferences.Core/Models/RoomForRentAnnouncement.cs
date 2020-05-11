using System;
using System.Collections.Generic;
using System.Linq;
using Riva.AnnouncementPreferences.Core.Enums;

namespace Riva.AnnouncementPreferences.Core.Models
{
    public class RoomForRentAnnouncement
    {
        public string SourceUrl { get; }
        public decimal? Price { get; }
        public IReadOnlyCollection<RoomType> RoomTypes { get; }
        public IReadOnlyCollection<Guid> CityDistricts { get; }

        public RoomForRentAnnouncement(string sourceUrl, decimal? price, IEnumerable<RoomType> roomTypes, IEnumerable<Guid> cityDistricts)
        {
            SourceUrl = sourceUrl;
            Price = price;
            RoomTypes = roomTypes.ToList().AsReadOnly();
            CityDistricts = cityDistricts.ToList().AsReadOnly();
        }
    }
}