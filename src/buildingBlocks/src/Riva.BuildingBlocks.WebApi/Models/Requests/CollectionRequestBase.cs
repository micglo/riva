using System.ComponentModel.DataAnnotations;
using Riva.BuildingBlocks.WebApi.ValidationAttributes;

namespace Riva.BuildingBlocks.WebApi.Models.Requests
{
    public abstract class CollectionRequestBase
    {
        [RequiredPropertyPair(nameof(PageSize))]
        [Range(1, int.MaxValue)]
        [Page(nameof(PageSize))]
        public int? Page { get; set; }

        [RequiredPropertyPair(nameof(Page))]
        [Range(1, 100)]
        public int? PageSize { get; set; }
    }
}