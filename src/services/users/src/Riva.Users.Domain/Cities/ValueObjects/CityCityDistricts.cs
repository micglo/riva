using System;
using System.Collections.Generic;
using System.Linq;
using Riva.Users.Domain.Cities.Exceptions;

namespace Riva.Users.Domain.Cities.ValueObjects
{
    public class CityCityDistricts
    {
        private readonly List<Guid> _cityDistricts;

        public CityCityDistricts(IEnumerable<Guid> cityDistricts)
        {
            if (cityDistricts is null)
                throw new CityCityDistrictsNullException();

            var cityDistrictsList = cityDistricts.ToList();

            if (cityDistrictsList.Any(x => x == Guid.Empty || x == new Guid()))
                throw new CityCityDistrictsInvalidsValueException();

            var anyDuplicates = cityDistrictsList.GroupBy(x => x).Any(g => g.Count() > 1);
            if (anyDuplicates)
                throw new CityCityDistrictsDuplicateValuesException();

            _cityDistricts = cityDistrictsList;
        }

        public static implicit operator List<Guid>(CityCityDistricts cityDistricts)
        {
            return cityDistricts._cityDistricts;
        }
    }
}