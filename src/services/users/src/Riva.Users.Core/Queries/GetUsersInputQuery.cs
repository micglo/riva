using Riva.BuildingBlocks.Core.Queries;

namespace Riva.Users.Core.Queries
{
    public class GetUsersInputQuery : CollectionInputQueryBase
    {
        public string Email { get; }
        public bool? ServiceActive { get; }

        public GetUsersInputQuery(int? page, int? pageSize, string sort, string email, bool? serviceActive) : base(page, pageSize, sort)
        {
            Email = email;
            ServiceActive = serviceActive;
        }
    }
}