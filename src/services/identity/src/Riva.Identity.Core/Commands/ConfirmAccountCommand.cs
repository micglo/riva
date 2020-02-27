using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.Identity.Core.Commands
{
    public class ConfirmAccountCommand : ICommand
    {
        public string Email { get; }
        public string Code { get; }

        public ConfirmAccountCommand(string email, string code)
        {
            Email = email;
            Code = code;
        }
    }
}