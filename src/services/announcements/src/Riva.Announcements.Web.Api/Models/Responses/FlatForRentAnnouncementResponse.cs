using System;
using System.Collections.Generic;
using System.Linq;
using Riva.Announcements.Web.Api.Models.Enums;
using Riva.BuildingBlocks.WebApi.Models.Responses;

namespace Riva.Announcements.Web.Api.Models.Responses
{
    public class FlatForRentAnnouncementResponse : ResponseBase
    {
        public string Title { get; }
        public string SourceUrl { get; }
        public Guid CityId { get; }
        public DateTimeOffset Created { get; }
        public string Description { get; }
        public decimal? Price { get; }
        public NumberOfRooms NumberOfRooms { get; }
        public IReadOnlyCollection<Guid> CityDistricts { get; }

        public FlatForRentAnnouncementResponse(Guid id, string title, string sourceUrl, Guid cityId, 
            DateTimeOffset created, string description, decimal? price, NumberOfRooms numberOfRooms, 
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