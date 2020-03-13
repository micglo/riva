using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Seeds;

namespace Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Configs
{
    public static class CityEntityConfig
    {
        public static void Configure(this EntityTypeBuilder<CityEntity> entity)
        {
            entity.Property(x => x.Name).IsRequired().HasMaxLength(256);
            entity.Property(x => x.PolishName).IsRequired().HasMaxLength(256);
            entity.Property(x => x.RowVersion).IsRequired().IsRowVersion();
            entity.HasIndex(x => new { x.Name, x.StateId }).IsUnique();
            entity.HasIndex(x => new { x.PolishName, x.StateId }).IsUnique();

            entity
                .HasOne(x => x.State)
                .WithMany(x => x.Cities)
                .HasForeignKey(x => x.StateId);

            entity.HasData(CityEntitySeeder.CityEntities);
        }
    }
}