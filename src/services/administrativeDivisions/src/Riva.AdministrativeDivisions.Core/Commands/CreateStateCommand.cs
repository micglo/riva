using System;
using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.AdministrativeDivisions.Core.Commands
{
    public class CreateStateCommand : ICommand
    {
        public Guid StateId { get; }
        public string Name { get; }
        public string PolishName { get; }

        public CreateStateCommand(Guid stateId, string name, string polishName)
        {
            StateId = stateId;
            Name = name;
            PolishName = polishName;
        }
    }
}