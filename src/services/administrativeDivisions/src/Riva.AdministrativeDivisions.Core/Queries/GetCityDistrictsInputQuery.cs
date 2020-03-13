using System;
using System.Collections.Generic;
using System.Linq;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.AdministrativeDivisions.Core.Queries
{
    public class GetCityDistrictsInputQuery : CollectionInputQueryBase
    {
        public string Name { get; }
        public string PolishName { get; }
        public Guid? CityId { get; }
        public Guid? ParentId { get; }
        public IReadOnlyCollection<Guid> CityIds { get; }

        public GetCityDistrictsInputQuery(int? page, int? pageSize, string sort, string name,
            string polishName, Guid? cityId, Guid? parentId, IEnumerable<Guid> cityIds) : base(page, pageSize, sort)
        {
            Name = name;
            PolishName = polishName;
            CityId = cityId;
            ParentId = parentId;
            CityIds = cityIds.ToList().AsReadOnly();
        }
    }
}