using System;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.Identity.Core.Queries
{
    public class GetRoleInputQuery : IInputQuery
    {
        public Guid RoleId { get; }

        public GetRoleInputQuery(Guid roleId)
        {
            RoleId = roleId;
        }
    }
}