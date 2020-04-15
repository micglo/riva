using System;
using System.Collections.Generic;
using Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Models;

namespace Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Entities
{
    public class CityDistrictEntity : EntityBase
    {
        public Guid CityId { get; set; }
        public CityEntity City { get; set; }
        public ICollection<AnnouncementPreferenceCityDistrictEntity> AnnouncementPreferenceCityDistricts { get; set; }

        public CityDistrictEntity()
        {
            AnnouncementPreferenceCityDistricts = new List<AnnouncementPreferenceCityDistrictEntity>();
        }
    }
}