using System;
using System.Collections.Generic;
using System.Linq;
using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.AdministrativeDivisions.Core.Commands
{
    public class UpdateStateCommand : ICommand
    {
        public Guid StateId { get; }
        public string Name { get; }
        public string PolishName { get; }
        public IReadOnlyCollection<byte> RowVersion { get; }

        public UpdateStateCommand(Guid stateId, IEnumerable<byte> rowVersion, string name, string polishName)
        {
            StateId = stateId;
            Name = name;
            PolishName = polishName;
            RowVersion = rowVersion.ToList().AsReadOnly();
        }
    }
}