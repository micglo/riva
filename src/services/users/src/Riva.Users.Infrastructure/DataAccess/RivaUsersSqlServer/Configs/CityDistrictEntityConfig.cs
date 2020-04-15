using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Entities;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Seeds;

namespace Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Configs
{
    public static class CityDistrictEntityConfig
    {
        public static void Configure(this EntityTypeBuilder<CityDistrictEntity> entity)
        {
            entity
                .HasOne(x => x.City)
                .WithMany(x => x.CityDistricts)
                .HasForeignKey(x => x.CityId)
                .IsRequired();

            entity.HasData(WroclawCityDistrictEntitySeeder.WroclawCityDistrictEntities);
        }
    }
}