using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Riva.Announcements.Web.Api.Models.Enums;
using Riva.BuildingBlocks.WebApi.ValidationAttributes;

namespace Riva.Announcements.Web.Api.Models.Requests
{
    public class CreateFlatForRentAnnouncementRequest
    {
        public CreateFlatForRentAnnouncementRequest()
        {
            CityDistricts = new List<Guid>();
        }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(256)]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string SourceUrl { get; set; }

        [Required]
        public Guid CityId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Description { get; set; }

        public decimal? Price { get; set; }

        [Required]
        public NumberOfRooms NumberOfRooms { get; set; }

        [GuidCollection(true, false, false)]
        public IEnumerable<Guid> CityDistricts { get; set; }
    }
}