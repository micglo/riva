using System;
using System.Collections.Generic;
using Riva.Identity.Domain.Roles.ValueObjects;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Roles.Aggregates
{
    public class Role : VersionedAggregateBase
    {
        public string Name { get; private set; }

        public Role(Guid id, IEnumerable<byte> rowVersion, string name) : base(id, rowVersion)
        {
            Name = new RoleName(name);
        }

        public void ChangeName(string name)
        {
            Name = new RoleName(name);
        }
    }
}