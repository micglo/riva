using System;
using System.Collections.Generic;
using System.Linq;
using Riva.Announcements.Web.Api.Models.Enums;
using Riva.BuildingBlocks.WebApi.Models.Responses;

namespace Riva.Announcements.Web.Api.Models.Responses
{
    public class RoomForRentAnnouncementResponse : ResponseBase
    {
        public string Title { get; }
        public string SourceUrl { get; }
        public Guid CityId { get; }
        public DateTimeOffset Created { get; }
        public string Description { get; }
        public decimal? Price { get; }
        public IReadOnlyCollection<RoomType> RoomTypes { get; }
        public IReadOnlyCollection<Guid> CityDistricts { get; }

        public RoomForRentAnnouncementResponse(Guid id, string title, string sourceUrl,
            Guid cityId, DateTimeOffset created, string description, decimal? price, 
            IEnumerable<RoomType> roomTypes, IEnumerable<Guid> cityDistricts) : base(id)
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