using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.Identity.Core.Commands
{
    public class RequestAccountConfirmationTokenCommand : ICommand
    {
        public string Email { get; }

        public RequestAccountConfirmationTokenCommand(string email)
        {
            Email = email;
        }
    }
}