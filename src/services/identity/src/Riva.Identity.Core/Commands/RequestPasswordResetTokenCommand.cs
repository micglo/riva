using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.Identity.Core.Commands
{
    public class RequestPasswordResetTokenCommand : ICommand
    {
        public string Email { get; }

        public RequestPasswordResetTokenCommand(string email)
        {
            Email = email;
        }
    }
}