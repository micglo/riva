using System;
using System.Collections.Generic;
using System.Linq;
using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.AdministrativeDivisions.Core.Commands
{
    public class UpdateCityCommand : ICommand
    {
        public Guid CityId { get; }
        public string Name { get; }
        public string PolishName { get; }
        public Guid StateId { get; }
        public IReadOnlyCollection<byte> RowVersion { get; }

        public UpdateCityCommand(Guid cityId, IEnumerable<byte> rowVersion, string name, string polishName, Guid stateId)
        {
            CityId = cityId;
            RowVersion = rowVersion.ToList().AsReadOnly();
            Name = name;
            PolishName = polishName;
            StateId = stateId;
        }
    }
}