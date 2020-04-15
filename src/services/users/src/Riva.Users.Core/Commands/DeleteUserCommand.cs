using System;
using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.Users.Core.Commands
{
    public class DeleteUserCommand : ICommand
    {
        public Guid UserId { get; }

        public DeleteUserCommand(Guid userId)
        {
            UserId = userId;
        }
    }
}