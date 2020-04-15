using System;
using System.Collections.Generic;
using Riva.BuildingBlocks.Domain;
using Riva.Users.Domain.Cities.ValueObjects;

namespace Riva.Users.Domain.Cities.Aggregates
{
    public class City : AggregateBase
    {
        private readonly List<Guid> _cityDistricts;
        public IReadOnlyCollection<Guid> CityDistricts => _cityDistricts.AsReadOnly();

        public City(Guid id, IEnumerable<Guid> cityDistricts) : base(id)
        {
            _cityDistricts = new CityCityDistricts(cityDistricts);
        }
    }
}