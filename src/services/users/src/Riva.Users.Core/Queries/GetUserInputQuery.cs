using System;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.Users.Core.Queries
{
    public class GetUserInputQuery : IInputQuery
    {
        public Guid UserId { get; }

        public GetUserInputQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}