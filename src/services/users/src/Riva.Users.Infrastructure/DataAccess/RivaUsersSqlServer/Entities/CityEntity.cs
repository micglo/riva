using System.Collections.Generic;
using Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Models;

namespace Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Entities
{
    public class CityEntity : EntityBase
    {
        public ICollection<CityDistrictEntity> CityDistricts { get; set; }

        public CityEntity()
        {
            CityDistricts = new List<CityDistrictEntity>();
        }
    }
}