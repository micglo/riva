using System;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.AdministrativeDivisions.Core.Queries
{
    public class GetCityInputQuery : IInputQuery
    {
        public Guid CityId { get; }

        public GetCityInputQuery(Guid cityId)
        {
            CityId = cityId;
        }
    }
}