using System;
using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.Identity.Core.Commands
{
    public class CreateRoleCommand : ICommand
    {
        public Guid RoleId { get; }
        public string Name { get; }

        public CreateRoleCommand(Guid roleId, string name)
        {
            RoleId = roleId;
            Name = name;
        }
    }
}