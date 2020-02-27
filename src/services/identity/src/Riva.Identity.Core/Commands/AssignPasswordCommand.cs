using System;
using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.Identity.Core.Commands
{
    public class AssignPasswordCommand : ICommand
    {
        public Guid AccountId { get; }
        public string Password { get; }

        public AssignPasswordCommand(Guid accountId, string password)
        {
            AccountId = accountId;
            Password = password;
        }
    }
}