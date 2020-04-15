using System;
using System.Collections.Generic;
using System.Linq;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.Users.Core.Queries
{
    public class FlatForRentAnnouncementPreferenceOutputQuery : OutputQueryBase
    {
        public Guid CityId { get; }
        public decimal? PriceMin { get; }
        public decimal? PriceMax { get; }
        public int? RoomNumbersMin { get; }
        public int? RoomNumbersMax { get; }
        public IReadOnlyCollection<Guid> CityDistricts { get; }

        public FlatForRentAnnouncementPreferenceOutputQuery(Guid id, Guid cityId, decimal? priceMin, 
            decimal? priceMax, int? roomNumbersMin, int? roomNumbersMax, IEnumerable<Guid> cityDistricts) : base(id)
        {
            CityId = cityId;
            PriceMin = priceMin;
            PriceMax = priceMax;
            RoomNumbersMin = roomNumbersMin;
            RoomNumbersMax = roomNumbersMax;
            CityDistricts = cityDistricts.ToList().AsReadOnly();
        }
    }
}