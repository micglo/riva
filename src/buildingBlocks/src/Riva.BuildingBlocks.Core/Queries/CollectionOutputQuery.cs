using System.Collections.Generic;
using System.Linq;

namespace Riva.BuildingBlocks.Core.Queries
{
    public class CollectionOutputQuery<T> : IOutputQuery where T : OutputQueryBase
    {
        public long TotalCount { get; }
        public IReadOnlyCollection<T> Results { get; }

        public CollectionOutputQuery(long totalCount, IEnumerable<T> results)
        {
            TotalCount = totalCount;
            Results = results.ToList().AsReadOnly();
        }
    }
}