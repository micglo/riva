using System;
using System.Collections.Generic;
using System.Linq;
using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.Identity.Core.Commands
{
    public class DeleteRoleCommand : ICommand
    {
        public Guid RoleId { get; }
        public IReadOnlyCollection<byte> RowVersion { get; }

        public DeleteRoleCommand(Guid roleId, IEnumerable<byte> rowVersion)
        {
            RoleId = roleId;
            RowVersion = rowVersion.ToList().AsReadOnly();
        }
    }
}