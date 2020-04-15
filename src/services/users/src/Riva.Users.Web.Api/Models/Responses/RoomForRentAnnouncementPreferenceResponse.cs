using System;
using System.Collections.Generic;
using System.Linq;
using Riva.BuildingBlocks.WebApi.Models.Responses;
using Riva.Users.Core.Enums;

namespace Riva.Users.Web.Api.Models.Responses
{
    public class RoomForRentAnnouncementPreferenceResponse : ResponseBase
    {
        public Guid CityId { get; }
        public decimal? PriceMin { get; }
        public decimal? PriceMax { get; }
        public RoomType? RoomType { get; }
        public IReadOnlyCollection<Guid> CityDistricts { get; }

        public RoomForRentAnnouncementPreferenceResponse(Guid id, Guid cityId, decimal? priceMin, 
            decimal? priceMax, RoomType? roomType, IEnumerable<Guid> cityDistricts) : base(id)
        {
            PriceMin = priceMin;
            PriceMax = priceMax;
            CityId = cityId;
            RoomType = roomType;
            CityDistricts = cityDistricts.ToList().AsReadOnly();
        }
    }
}