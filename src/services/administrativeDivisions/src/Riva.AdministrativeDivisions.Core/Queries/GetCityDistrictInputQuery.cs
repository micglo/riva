using System;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.AdministrativeDivisions.Core.Queries
{
    public class GetCityDistrictInputQuery : IInputQuery
    {
        public Guid CityDistrictId { get; }

        public GetCityDistrictInputQuery(Guid cityDistrictId)
        {
            CityDistrictId = cityDistrictId;
        }
    }
}