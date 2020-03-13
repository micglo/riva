using System;
using System.Collections.Generic;
using System.Linq;
using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.AdministrativeDivisions.Core.Commands
{
    public class DeleteStateCommand : ICommand
    {
        public Guid StateId { get; }
        public IReadOnlyCollection<byte> RowVersion { get; }

        public DeleteStateCommand(Guid stateId, IEnumerable<byte> rowVersion)
        {
            StateId = stateId;
            RowVersion = rowVersion.ToList().AsReadOnly();
        }
    }
}