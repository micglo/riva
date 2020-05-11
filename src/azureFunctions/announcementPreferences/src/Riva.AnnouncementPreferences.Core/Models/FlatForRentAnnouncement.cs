using System;
using System.Collections.Generic;
using System.Linq;
using Riva.AnnouncementPreferences.Core.Enums;

namespace Riva.AnnouncementPreferences.Core.Models
{
    public class FlatForRentAnnouncement
    {
        public string SourceUrl { get; }
        public decimal? Price { get; }
        public NumberOfRooms? NumberOfRooms { get; }
        public IReadOnlyCollection<Guid> CityDistricts { get; }

        public FlatForRentAnnouncement(string sourceUrl, decimal? price, NumberOfRooms? numberOfRooms, IEnumerable<Guid> cityDistricts)
        {
            SourceUrl = sourceUrl;
            Price = price;
            NumberOfRooms = numberOfRooms;
            CityDistricts = cityDistricts.ToList().AsReadOnly();
        }
    }
}