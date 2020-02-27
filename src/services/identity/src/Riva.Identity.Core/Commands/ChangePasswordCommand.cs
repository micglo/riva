using System;
using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.Identity.Core.Commands
{
    public class ChangePasswordCommand : ICommand
    {
        public Guid AccountId { get; }
        public string OldPassword { get; }
        public string NewPassword { get; }

        public ChangePasswordCommand(Guid accountId, string oldPassword, string newPassword)
        {
            AccountId = accountId;
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }
    }
}