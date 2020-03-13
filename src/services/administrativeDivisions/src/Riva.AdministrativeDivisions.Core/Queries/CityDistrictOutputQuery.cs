using System;
using System.Collections.Generic;
using System.Linq;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.AdministrativeDivisions.Core.Queries
{
    public class CityDistrictOutputQuery : VersionedOutputQueryBase
    {
        public string Name { get; }
        public string PolishName { get; }
        public Guid CityId { get; }
        public Guid? ParentId { get; }
        public IReadOnlyCollection<string> NameVariants { get; }

        public CityDistrictOutputQuery(Guid id, IEnumerable<byte> rowVersion, string name,
            string polishName, Guid cityId, Guid? parentId, IEnumerable<string> nameVariants) : base(id, rowVersion)
        {
            Name = name;
            PolishName = polishName;
            CityId = cityId;
            ParentId = parentId;
            NameVariants = nameVariants.ToList().AsReadOnly();
        }
    }
}