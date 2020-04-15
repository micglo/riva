using System;
using System.Collections.Generic;
using System.Linq;
using Riva.BuildingBlocks.Core.Queries;
using Riva.Users.Domain.Users.Enumerations;

namespace Riva.Users.Core.Queries
{
    public class RoomForRentAnnouncementPreferenceOutputQuery : OutputQueryBase
    {
        public Guid CityId { get; }
        public decimal? PriceMin { get; }
        public decimal? PriceMax { get; }
        public RoomTypeEnumeration RoomType { get; }
        public IReadOnlyCollection<Guid> CityDistricts { get; }

        public RoomForRentAnnouncementPreferenceOutputQuery(Guid id, Guid cityId, decimal? priceMin, decimal? priceMax,
            RoomTypeEnumeration roomType, IEnumerable<Guid> cityDistricts) : base(id)
        {
            PriceMin = priceMin;
            PriceMax = priceMax;
            CityId = cityId;
            RoomType = roomType;
            CityDistricts = cityDistricts.ToList().AsReadOnly();
        }
    }
}