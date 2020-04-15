using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Entities;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Seeds;

namespace Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Configs
{
    public static class CityEntityConfig
    {
        public static void Configure(this EntityTypeBuilder<CityEntity> entity)
        {
            entity.HasData(CityEntitySeeder.CityEntities);
        }
    }
}