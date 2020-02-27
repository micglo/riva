using System.Collections.Generic;
using System.Linq;

namespace Riva.BuildingBlocks.WebApi.Models.Responses
{
    public class CollectionResponse<T> where T : ResponseBase
    {
        public long TotalCount { get; }
        public IReadOnlyCollection<T> Results { get; }

        public CollectionResponse(long totalCount, IEnumerable<T> results)
        {
            TotalCount = totalCount;
            Results = results.ToList().AsReadOnly();
        }
    }
}