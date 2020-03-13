using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities;

namespace Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Seeds
{
    public static class CityEntitySeeder
    {
        public static Guid WroclawId = new Guid("1D02CE58-3BFE-46BC-AA61-D2DE1C1690C1");

        public static IEnumerable<CityEntity> CityEntities = new Collection<CityEntity>
        {
            new CityEntity
            {
                Id = WroclawId,
                Name = "Wroclaw",
                PolishName = "Wrocław",
                StateId = StateEntitySeeder.DolnoslaskieId
            }
        };
    }
}