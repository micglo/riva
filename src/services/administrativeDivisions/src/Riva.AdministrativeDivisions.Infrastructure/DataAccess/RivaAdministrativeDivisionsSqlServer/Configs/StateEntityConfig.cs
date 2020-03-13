using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Seeds;

namespace Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Configs
{
    public static class StateEntityConfig
    {
        public static void Configure(this EntityTypeBuilder<StateEntity> entity)
        {
            entity.Property(x => x.Name).IsRequired().HasMaxLength(256);
            entity.Property(x => x.PolishName).IsRequired().HasMaxLength(256);
            entity.Property(x => x.RowVersion).IsRequired().IsRowVersion();
            entity.HasIndex(x => x.Name).IsUnique();
            entity.HasIndex(x => x.PolishName).IsUnique();

            entity.HasData(StateEntitySeeder.StateEntities);
        }
    }
}