using System;
using System.Collections.Generic;
using Cosmonaut;
using Cosmonaut.Attributes;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Aggregates;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Enums;

namespace Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Entities
{
    [SharedCosmosCollection("announcements", nameof(RoomForRentAnnouncement))]
    public class RoomForRentAnnouncementEntity : ISharedCosmosEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string SourceUrl { get; set; }
        public Guid CityId { get; set; }
        public DateTimeOffset Created { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public IEnumerable<Guid> CityDistricts { get; set; }
        public IEnumerable<RoomType> RoomTypes { get; set; }
        public string CosmosEntityName { get; set; }

        public RoomForRentAnnouncementEntity()
        {
            CityDistricts = new List<Guid>();
            RoomTypes = new List<RoomType>();
        }
    }
}