using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Seeds;

namespace Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Configs
{
    public static class CityDistrictNameVariantEntityConfig
    {
        public static void Configure(this EntityTypeBuilder<CityDistrictNameVariantEntity> entity)
        {
            entity.Property(x => x.Value).IsRequired().HasMaxLength(256);
            entity.HasIndex(x => new {x.Value, x.CityDistrictId}).IsUnique();

            entity
                .HasOne(x => x.CityDistrict)
                .WithMany(x => x.NameVariants)
                .HasForeignKey(x => x.CityDistrictId);

            entity.HasData(WroclawCityDistrictNameVariantEntitySeeder.WroclawCityDistrictNameVariantEntities);
        }
    }
}