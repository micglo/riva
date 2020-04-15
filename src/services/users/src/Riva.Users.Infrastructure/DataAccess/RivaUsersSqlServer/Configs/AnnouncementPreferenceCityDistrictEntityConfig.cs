using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Entities;

namespace Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Configs
{
    public static class AnnouncementPreferenceCityDistrictEntityConfig
    {
        public static void Configure(this EntityTypeBuilder<AnnouncementPreferenceCityDistrictEntity> entity)
        {
            entity.ToTable("AnnouncementPreferenceCityDistricts");
            entity.HasKey(x => new { x.AnnouncementPreferenceId, x.CityDistrictId });

            entity
                .HasOne(x => x.AnnouncementPreference)
                .WithMany(x => x.AnnouncementPreferenceCityDistricts)
                .HasForeignKey(x => x.AnnouncementPreferenceId)
                .IsRequired();

            entity
                .HasOne(x => x.CityDistrict)
                .WithMany(x => x.AnnouncementPreferenceCityDistricts)
                .HasForeignKey(x => x.CityDistrictId)
                .IsRequired();
        }
    }
}