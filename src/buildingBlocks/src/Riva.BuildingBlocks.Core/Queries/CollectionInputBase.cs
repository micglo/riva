using Riva.BuildingBlocks.Core.Exceptions;

namespace Riva.BuildingBlocks.Core.Queries
{
    public abstract class CollectionInputQueryBase : IInputQuery
    {
        public int? Page { get; }
        public int? PageSize { get; }
        public string Sort { get; }

        protected CollectionInputQueryBase(int? page, int? pageSize, string sort)
        {
            if (pageSize.HasValue && !page.HasValue)
                throw new PageNullException();
            if (page.HasValue && !pageSize.HasValue)
                throw new PageSizeNullException();

            Page = page;
            PageSize = pageSize;
            Sort = sort;
        }
    }
}