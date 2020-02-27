using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.Identity.Core.Commands
{
    public class ResetPasswordCommand : ICommand
    {
        public string Email { get; }
        public string Code { get; }
        public string Password { get; }

        public ResetPasswordCommand(string email, string code, string password)
        {
            Email = email;
            Code = code;
            Password = password;
        }
    }
}