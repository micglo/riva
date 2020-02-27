using System;
using System.Collections.Generic;
using System.Linq;

namespace Riva.BuildingBlocks.WebApi.Models.Responses
{
    public abstract class VersionedResponseBase : ResponseBase
    {
        public IReadOnlyCollection<byte> RowVersion { get; }

        protected VersionedResponseBase(Guid id, IEnumerable<byte> rowVersion) : base(id)
        {
            RowVersion = rowVersion.ToArray();
        }
    }
}