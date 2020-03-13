using System.Collections.Generic;
using System.Collections.ObjectModel;
using Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Models;

namespace Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities
{
    public class StateEntity : VersionedEntityBase
    {
        public string Name { get; set; }
        public string PolishName { get; set; }
        public ICollection<CityEntity> Cities { get; set; }

        public StateEntity()
        {
            Cities = new Collection<CityEntity>();
        }
    }
}