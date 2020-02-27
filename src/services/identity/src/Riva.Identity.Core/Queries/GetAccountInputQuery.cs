using System;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.Identity.Core.Queries
{
    public class GetAccountInputQuery : IInputQuery
    {
        public Guid AccountId { get; }

        public GetAccountInputQuery(Guid accountId)
        {
            AccountId = accountId;
        }
    }
}