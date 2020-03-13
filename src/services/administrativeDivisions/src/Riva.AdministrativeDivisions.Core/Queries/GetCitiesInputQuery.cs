using System;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.AdministrativeDivisions.Core.Queries
{
    public class GetCitiesInputQuery : CollectionInputQueryBase
    {
        public Guid? StateId { get; }
        public string Name { get; }
        public string PolishName { get; }

        public GetCitiesInputQuery(int? page, int? pageSize, string sort, Guid? stateId, string name, string polishName) : base(page, pageSize, sort)
        {
            StateId = stateId;
            Name = name;
            PolishName = polishName;
        }
    }
}