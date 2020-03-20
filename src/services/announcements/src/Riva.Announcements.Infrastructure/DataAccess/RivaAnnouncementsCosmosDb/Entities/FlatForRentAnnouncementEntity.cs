using System;
using System.Collections.Generic;
using Cosmonaut;
using Cosmonaut.Attributes;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Aggregates;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Enums;

namespace Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Entities
{
    [SharedCosmosCollection("announcements", nameof(FlatForRentAnnouncement))]
    public class FlatForRentAnnouncementEntity : ISharedCosmosEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string SourceUrl { get; set; }
        public Guid CityId { get; set; }
        public DateTimeOffset Created { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public NumberOfRooms NumberOfRooms { get; set; }
        public IEnumerable<Guid> CityDistricts { get; set; }
        public string CosmosEntityName { get; set; }

        public FlatForRentAnnouncementEntity()
        {
            CityDistricts = new List<Guid>();
        }
    }
}