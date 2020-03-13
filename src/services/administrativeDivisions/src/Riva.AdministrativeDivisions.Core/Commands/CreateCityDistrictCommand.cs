﻿using System;
using System.Collections.Generic;
using System.Linq;
using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.AdministrativeDivisions.Core.Commands
{
    public class CreateCityDistrictCommand : ICommand
    {
        public Guid CityDistrictId { get; }
        public string Name { get; }
        public string PolishName { get; }
        public Guid CityId { get; }
        public Guid? ParentId { get; }
        public IReadOnlyCollection<string> NameVariants { get; }

        public CreateCityDistrictCommand(Guid cityDistrictId, string name, string polishName, Guid cityId, Guid? parentId, IEnumerable<string> nameVariants)
        {
            CityDistrictId = cityDistrictId;
            Name = name;
            PolishName = polishName;
            CityId = cityId;
            ParentId = parentId;
            NameVariants = nameVariants.ToList().AsReadOnly();
        }
    }
}