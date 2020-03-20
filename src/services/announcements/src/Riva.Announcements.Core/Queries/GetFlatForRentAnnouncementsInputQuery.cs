using System;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Enumerations;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.Announcements.Core.Queries
{
    public class GetFlatForRentAnnouncementsInputQuery : CollectionInputQueryBase
    {
        public DateTimeOffset? CreatedFrom { get; }
        public DateTimeOffset? CreatedTo { get; }
        public Guid? CityId { get; }
        public decimal? PriceFrom { get; }
        public decimal? PriceTo { get; }
        public NumberOfRoomsEnumeration NumberOfRooms { get; }
        public Guid? CityDistrict { get; }

        public GetFlatForRentAnnouncementsInputQuery(int? page, int? pageSize, string sort, DateTimeOffset? createdFrom, 
            DateTimeOffset? createdTo, Guid? cityId, decimal? priceFrom, decimal? priceTo, NumberOfRoomsEnumeration numberOfRooms, 
            Guid? cityDistrict) : base(page, pageSize, sort)
        {
            CreatedFrom = createdFrom;
            CreatedTo = createdTo;
            CityId = cityId;
            PriceFrom = priceFrom;
            PriceTo = priceTo;
            NumberOfRooms = numberOfRooms;
            CityDistrict = cityDistrict;
        }
    }
}