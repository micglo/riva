using System;
using System.Collections.Generic;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.Identity.Core.Queries
{
    public class RoleOutputQuery : VersionedOutputQueryBase
    {
        public string Name { get; }

        public RoleOutputQuery(Guid id, IEnumerable<byte> rowVersion, string name) : base(id, rowVersion)
        {
            Name = name;
        }
    }
}