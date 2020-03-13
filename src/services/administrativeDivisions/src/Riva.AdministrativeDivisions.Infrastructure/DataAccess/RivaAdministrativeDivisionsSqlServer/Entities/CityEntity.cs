using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Models;

namespace Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities
{
    public class CityEntity : VersionedEntityBase
    {
        public string Name { get; set; }
        public string PolishName { get; set; }
        public Guid StateId { get; set; }
        public StateEntity State { get; set; }
        public ICollection<CityDistrictEntity> CityDistricts { get; set; }

        public CityEntity()
        {
            CityDistricts = new Collection<CityDistrictEntity>();
        }
    }
}