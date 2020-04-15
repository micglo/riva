using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Riva.BuildingBlocks.WebApi.ValidationAttributes;
using Riva.Users.Web.Api.ValidationAttributes;

namespace Riva.Users.Web.Api.Models.Requests
{
    public class CreateFlatForRentAnnouncementPreferenceRequest
    {
        [Required]
        public Guid CityId { get; set; }

        [Range(0.0, double.MaxValue)]
        public decimal? PriceMin { get; set; }

        [Range(0.0, double.MaxValue)]
        [PriceMax]
        public decimal? PriceMax { get; set; }

        [Range(1, int.MaxValue)]
        public int? RoomNumbersMin { get; set; }

        [Range(1, int.MaxValue)]
        [RoomNumbersMax]
        public int? RoomNumbersMax { get; set; }

        [GuidCollection(true, false, false)]
        public IEnumerable<Guid> CityDistricts { get; set; }
    }
}