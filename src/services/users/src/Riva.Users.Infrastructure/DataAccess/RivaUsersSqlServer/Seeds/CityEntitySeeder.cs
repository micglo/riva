using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Entities;

namespace Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Seeds
{
    public static class CityEntitySeeder
    {
        public static Guid WroclawId = new Guid("1D02CE58-3BFE-46BC-AA61-D2DE1C1690C1");

        public static IEnumerable<CityEntity> CityEntities = new Collection<CityEntity>
        {
            new CityEntity
            {
                Id = WroclawId
            }
        };
    }
}