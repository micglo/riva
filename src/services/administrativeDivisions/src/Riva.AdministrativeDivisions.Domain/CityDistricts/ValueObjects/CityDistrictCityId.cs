using System;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Exceptions;

namespace Riva.AdministrativeDivisions.Domain.CityDistricts.ValueObjects
{
    public class CityDistrictCityId
    {
        private readonly Guid _cityId;

        public CityDistrictCityId(Guid cityId)
        {
            if (cityId == Guid.Empty || Equals(cityId, new Guid?()) || cityId == new Guid())
                throw new CityDistrictCityIdNullException();

            _cityId = cityId;
        }

        public static implicit operator Guid(CityDistrictCityId cityId)
        {
            return cityId._cityId;
        }
    }
}