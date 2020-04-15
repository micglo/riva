using System;
using System.Collections.Generic;
using Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Models;
using Riva.Users.Core.Enums;

namespace Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Entities
{
    public class AnnouncementPreferenceEntity : EntityBase
    {
        public Guid UserId { get; set; }
        public Guid CityId { get; set; }
        public AnnouncementPreferenceType AnnouncementPreferenceType { get; set; }
        public RoomType? RoomType { get; set; }
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }
        public int? RoomNumbersMin { get; set; }
        public int? RoomNumbersMax { get; set; }
        public UserEntity User { get; set; }
        public ICollection<AnnouncementPreferenceCityDistrictEntity> AnnouncementPreferenceCityDistricts { get; set; }

        public AnnouncementPreferenceEntity()
        {
            AnnouncementPreferenceCityDistricts = new List<AnnouncementPreferenceCityDistrictEntity>();
        }
    }
}