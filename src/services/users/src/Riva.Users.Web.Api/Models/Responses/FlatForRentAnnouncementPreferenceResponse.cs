using System;
using System.Collections.Generic;
using System.Linq;
using Riva.BuildingBlocks.WebApi.Models.Responses;

namespace Riva.Users.Web.Api.Models.Responses
{
    public class FlatForRentAnnouncementPreferenceResponse : ResponseBase
    {
        public Guid CityId { get; }
        public decimal? PriceMin { get; }
        public decimal? PriceMax { get; }
        public int? RoomNumbersMin { get; }
        public int? RoomNumbersMax { get; }
        public IReadOnlyCollection<Guid> CityDistricts { get; }

        public FlatForRentAnnouncementPreferenceResponse(Guid id, Guid cityId, decimal? priceMin, decimal? priceMax, 
            int? roomNumbersMin, int? roomNumbersMax, IEnumerable<Guid> cityDistricts) : base(id)
        {
            PriceMin = priceMin;
            PriceMax = priceMax;
            RoomNumbersMin = roomNumbersMin;
            RoomNumbersMax = roomNumbersMax;
            CityId = cityId;
            CityDistricts = cityDistricts.ToList().AsReadOnly();
        }
    }
}