using System;

namespace Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Entities
{
    public class AnnouncementPreferenceCityDistrictEntity
    {
        public Guid AnnouncementPreferenceId { get; set; }
        public Guid CityDistrictId { get; set; }
        public AnnouncementPreferenceEntity AnnouncementPreference { get; set; }
        public CityDistrictEntity CityDistrict { get; set; }
    }
}