using Riva.BuildingBlocks.Core.Queries;

namespace Riva.AdministrativeDivisions.Core.Queries
{
    public class GetStatesInputQuery : CollectionInputQueryBase
    {
        public string Name { get; }
        public string PolishName { get; }

        public GetStatesInputQuery(int? page, int? pageSize, string sort, string name, string polishName) : base(page, pageSize, sort)
        {
            Name = name;
            PolishName = polishName;
        }
    }
}