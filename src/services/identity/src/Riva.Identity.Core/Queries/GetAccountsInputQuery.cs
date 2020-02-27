using Riva.BuildingBlocks.Core.Queries;

namespace Riva.Identity.Core.Queries
{
    public class GetAccountsInputQuery : CollectionInputQueryBase
    {
        public string Email { get; }
        public bool? Confirmed { get; }

        public GetAccountsInputQuery(int? page, int? pageSize, string sort, string email, bool? confirmed) : base(page, pageSize, sort)
        {
            Email = email;
            Confirmed = confirmed;
        }
    }
}