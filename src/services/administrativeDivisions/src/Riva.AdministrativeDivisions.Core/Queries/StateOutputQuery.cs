using System;
using System.Collections.Generic;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.AdministrativeDivisions.Core.Queries
{
    public class StateOutputQuery : VersionedOutputQueryBase
    {
        public string Name { get; }
        public string PolishName { get; }

        public StateOutputQuery(Guid id, IEnumerable<byte> rowVersion, string name, string polishName) : base(id, rowVersion)
        {
            Name = name;
            PolishName = polishName;
        }
    }
}