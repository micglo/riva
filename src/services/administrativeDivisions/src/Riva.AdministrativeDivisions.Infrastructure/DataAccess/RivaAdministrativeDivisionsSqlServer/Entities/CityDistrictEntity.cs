using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Models;

namespace Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities
{
    public class CityDistrictEntity : VersionedEntityBase
    {
        public string Name { get; set; }
        public string PolishName { get; set; }
        public Guid? ParentId { get; set; }
        public Guid CityId { get; set; }
        public CityEntity City { get; set; }
        public ICollection<CityDistrictNameVariantEntity> NameVariants { get; set; }

        public CityDistrictEntity()
        {
            NameVariants = new Collection<CityDistrictNameVariantEntity>();
        }
    }
}