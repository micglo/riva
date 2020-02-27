using System;
using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.Identity.Core.Commands
{
    public class CreateAccountCommand : ICommand
    {
        public Guid AccountId { get; }
        public string Email { get; }
        public string Password { get; }
        public Guid CorrelationId { get; }

        public CreateAccountCommand(string email, string password)
        {
            AccountId = Guid.NewGuid();
            Email = email;
            Password = password;
            CorrelationId = Guid.NewGuid();
        }
    }
}