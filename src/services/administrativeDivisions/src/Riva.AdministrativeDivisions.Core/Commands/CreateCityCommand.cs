using System;
using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.AdministrativeDivisions.Core.Commands
{
    public class CreateCityCommand : ICommand
    {
        public Guid CityId { get; }
        public string Name { get; }
        public string PolishName { get; }
        public Guid StateId { get; }

        public CreateCityCommand(Guid cityId, string name, string polishName, Guid stateId)
        {
            CityId = cityId;
            Name = name;
            PolishName = polishName;
            StateId = stateId;
        }
    }
}