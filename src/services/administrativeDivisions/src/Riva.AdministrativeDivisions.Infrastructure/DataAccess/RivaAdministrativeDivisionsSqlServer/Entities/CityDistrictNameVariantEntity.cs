using System;
using Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Models;

namespace Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities
{
    public class CityDistrictNameVariantEntity : EntityBase
    {
        public string Value { get; set; }
        public Guid CityDistrictId { get; set; }
        public CityDistrictEntity CityDistrict { get; set; }

        public CityDistrictNameVariantEntity()
        {
            Id = Guid.NewGuid();
        }
    }
}