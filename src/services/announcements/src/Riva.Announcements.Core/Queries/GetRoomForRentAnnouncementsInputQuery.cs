using System;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Enumerations;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.Announcements.Core.Queries
{
    public class GetRoomForRentAnnouncementsInputQuery : CollectionInputQueryBase
    {
        public DateTimeOffset? CreatedFrom { get; }
        public DateTimeOffset? CreatedTo { get; }
        public Guid? CityId { get; }
        public decimal? PriceFrom { get; }
        public decimal? PriceTo { get; }
        public RoomTypeEnumeration RoomType { get; }
        public Guid? CityDistrict { get; }

        public GetRoomForRentAnnouncementsInputQuery(int? page, int? pageSize, string sort, DateTimeOffset? createdFrom, 
            DateTimeOffset? createdTo, Guid? cityId, decimal? priceFrom, decimal? priceTo, RoomTypeEnumeration roomType, 
            Guid? cityDistrict) : base(page, pageSize, sort)
        {
            CreatedFrom = createdFrom;
            CreatedTo = createdTo;
            CityId = cityId;
            PriceFrom = priceFrom;
            PriceTo = priceTo;
            RoomType = roomType;
            CityDistrict = cityDistrict;
        }
    }
}