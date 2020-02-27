using System;
using System.Collections.Generic;
using System.Linq;

namespace Riva.BuildingBlocks.Core.Queries
{
    public abstract class VersionedOutputQueryBase : OutputQueryBase
    {
        public IReadOnlyCollection<byte> RowVersion { get; }

        protected VersionedOutputQueryBase(Guid id, IEnumerable<byte> rowVersion) : base(id)
        {
            RowVersion = rowVersion.ToList().AsReadOnly();
        }
    }
}