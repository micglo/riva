using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Riva.BuildingBlocks.WebApi.Models.Requests
{
    public abstract class VersionedUpdateRequestBase
    {
        [JsonIgnore]
        public IReadOnlyCollection<byte> RowVersion { get; private set; }

        public void SetRowVersion(IEnumerable<byte> rowVersion)
        {
            RowVersion = rowVersion.ToList().AsReadOnly();
        }
    }
}