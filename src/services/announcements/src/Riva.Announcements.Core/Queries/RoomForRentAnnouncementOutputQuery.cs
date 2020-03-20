using System;
using System.Collections.Generic;
using System.Linq;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Enumerations;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.Announcements.Core.Queries
{
    public class RoomForRentAnnouncementOutputQuery : OutputQueryBase
    {
        public string Title { get; }
        public string SourceUrl { get; }
        public Guid CityId { get; }
        public DateTimeOffset Created { get; }
        public string Description { get; }
        public decimal? Price { get; }
        public IReadOnlyCollection<RoomTypeEnumeration> RoomTypes { get; }
        public IReadOnlyCollection<Guid> CityDistricts { get; }

        public RoomForRentAnnouncementOutputQuery(Guid id, string title, string sourceUrl, Guid cityId, DateTimeOffset created, 
            string description, decimal? price, IEnumerable<RoomTypeEnumeration> roomTypes, IEnumerable<Guid> cityDistricts) : base(id)
        {
            Title = title;
            SourceUrl = sourceUrl;
            CityId = cityId;
            Created = created;
            Description = description;
            Price = price;
            RoomTypes = roomTypes.ToList().AsReadOnly();
            CityDistricts = cityDistricts.ToList().AsReadOnly();
        }
    }
}