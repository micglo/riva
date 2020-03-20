using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Riva.Announcements.Web.Api.Models.Enums;
using Riva.Announcements.Web.Api.ValidationAttributes;
using Riva.BuildingBlocks.WebApi.ValidationAttributes;

namespace Riva.Announcements.Web.Api.Models.Requests
{
    public class UpdateRoomForRentAnnouncementRequest
    {
        public UpdateRoomForRentAnnouncementRequest()
        {
            CityDistricts = new List<Guid>();
            RoomTypes = new List<RoomType>();
        }

        [Required]
        public Guid Id { get; set; }

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

        [RoomTypes]
        public IEnumerable<RoomType> RoomTypes { get; set; }

        [GuidCollection(true, false, false)]
        public IEnumerable<Guid> CityDistricts { get; set; }
    }
}