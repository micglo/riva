using System;
using Riva.Announcements.Web.Api.Models.Enums;
using Riva.BuildingBlocks.WebApi.Models.Requests;
using Riva.BuildingBlocks.WebApi.ValidationAttributes;

namespace Riva.Announcements.Web.Api.Models.Requests
{
    public class GetFlatForRentAnnouncementsRequest : CollectionRequestBase
    {
        public DateTimeOffset? CreatedFrom { get; set; }

        public DateTimeOffset? CreatedTo { get; set; }

        public Guid? CityId { get; set; }

        public decimal? PriceFrom { get; set; }

        public decimal? PriceTo { get; set; }

        public NumberOfRooms? NumberOfRooms { get; set; }

        public Guid? CityDistrict { get; set; }

        [AllowedValues("created:asc", "created:desc", "price:asc", "price:desc")]
        public string Sort { get; set; }
    }
}