using System;
using System.Collections.Generic;
using System.Linq;
using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.AdministrativeDivisions.Core.Commands
{
    public class DeleteCityDistrictCommand : ICommand
    {
        public Guid CityDistrictId { get; }
        public IReadOnlyCollection<byte> RowVersion { get; }

        public DeleteCityDistrictCommand(Guid cityDistrictId, IEnumerable<byte> rowVersion)
        {
            CityDistrictId = cityDistrictId;
            RowVersion = rowVersion.ToList().AsReadOnly();
        }
    }
}