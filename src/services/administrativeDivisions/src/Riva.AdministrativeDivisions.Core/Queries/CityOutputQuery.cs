using System;
using System.Collections.Generic;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.AdministrativeDivisions.Core.Queries
{
    public class CityOutputQuery : VersionedOutputQueryBase
    {
        public string Name { get; }
        public string PolishName { get; }
        public Guid StateId { get; }

        public CityOutputQuery(Guid id, IEnumerable<byte> rowVersion, string name, string polishName, Guid stateId) : base(id, rowVersion)
        {
            Name = name;
            PolishName = polishName;
            StateId = stateId;
        }
    }
}