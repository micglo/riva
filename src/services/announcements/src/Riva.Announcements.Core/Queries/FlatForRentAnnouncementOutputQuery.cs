using System;
using System.Collections.Generic;
using System.Linq;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Enumerations;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.Announcements.Core.Queries
{
    public class FlatForRentAnnouncementOutputQuery : OutputQueryBase
    {
        public string Title { get; }
        public string SourceUrl { get; }
        public Guid CityId { get; }
        public DateTimeOffset Created { get; }
        public string Description { get; }
        public decimal? Price { get; }
        public NumberOfRoomsEnumeration NumberOfRooms { get; }
        public IReadOnlyCollection<Guid> CityDistricts { get; }

        public FlatForRentAnnouncementOutputQuery(Guid id, string title, string sourceUrl, Guid cityId, 
            DateTimeOffset created, string description, decimal? price, NumberOfRoomsEnumeration numberOfRooms, 
            IEnumerable<Guid> cityDistricts) : base(id)
        {
            Title = title;
            SourceUrl = sourceUrl;
            CityId = cityId;
            Created = created;
            Description = description;
            Price = price;
            NumberOfRooms = numberOfRooms;
            CityDistricts = cityDistricts.ToList().AsReadOnly();
        }
    }
}