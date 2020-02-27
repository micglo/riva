using System;
using System.Collections.Generic;
using System.Linq;
using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.Identity.Core.Commands
{
    public class UpdateAccountRolesCommand : ICommand
    {
        public Guid AccountId { get; }
        public IReadOnlyCollection<Guid> Roles { get; }

        public UpdateAccountRolesCommand(Guid accountId, IEnumerable<Guid> roles)
        {
            AccountId = accountId;
            Roles = roles.ToList().AsReadOnly();
        }
    }
}