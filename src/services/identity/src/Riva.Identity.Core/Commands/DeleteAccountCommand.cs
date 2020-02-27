using System;
using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.Identity.Core.Commands
{
    public class DeleteAccountCommand : ICommand
    {
        public Guid AccountId { get; }
        public Guid CorrelationId { get; }

        public DeleteAccountCommand(Guid accountId)
        {
            AccountId = accountId;
            CorrelationId = Guid.NewGuid();
        }
    }
}