using System;
using System.Collections.Generic;
using System.Linq;
using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.AdministrativeDivisions.Core.Commands
{
    public class DeleteCityCommand : ICommand
    {
        public Guid CityId { get; }
        public IReadOnlyCollection<byte> RowVersion { get; }

        public DeleteCityCommand(Guid cityId, IEnumerable<byte> rowVersion)
        {
            CityId = cityId;
            RowVersion = rowVersion.ToList().AsReadOnly();
        }
    }
}