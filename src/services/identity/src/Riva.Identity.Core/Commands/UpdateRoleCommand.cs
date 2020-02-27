using System;
using System.Collections.Generic;
using System.Linq;
using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.Identity.Core.Commands
{
    public class UpdateRoleCommand : ICommand
    {
        public Guid RoleId { get; }
        public string Name { get; }
        public IReadOnlyCollection<byte> RowVersion { get; }

        public UpdateRoleCommand(Guid roleId, IEnumerable<byte> rowVersion, string name)
        {
            RoleId = roleId;
            Name = name;
            RowVersion = rowVersion.ToList().AsReadOnly();
        }
    }
}