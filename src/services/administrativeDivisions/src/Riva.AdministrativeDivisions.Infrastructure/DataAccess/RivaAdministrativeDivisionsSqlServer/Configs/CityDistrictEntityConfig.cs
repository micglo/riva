using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Seeds;

namespace Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Configs
{
    public static class CityDistrictEntityConfig
    {

        public static void Configure(this EntityTypeBuilder<CityDistrictEntity> entity)
        {
            entity.Property(x => x.Name).IsRequired().HasMaxLength(256);
            entity.Property(x => x.PolishName).IsRequired().HasMaxLength(256);
            entity.Property(x => x.RowVersion).IsRequired().IsRowVersion();
            entity.HasIndex(x => new { x.Name, x.ParentId, x.CityId }).IsUnique();
            entity.HasIndex(x => new { x.PolishName, x.ParentId, x.CityId }).IsUnique();

            entity
                .HasOne(x => x.City)
                .WithMany(x => x.CityDistricts)
                .HasForeignKey(x => x.CityId);

            entity.HasData(WroclawCityDistrictEntitySeeder.WroclawCityDistrictEntities);
        }
    }
}