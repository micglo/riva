using System;
using System.Collections.Generic;
using System.Linq;
using Riva.Users.Domain.Users.Exceptions.EntityExceptions.FlatForRentAnnouncementPreferenceExceptions;

namespace Riva.Users.Domain.Users.ValueObjects.EntityValueObjects.FlatForRentAnnouncementPreferenceValueObjects
{
    public class FlatForRentAnnouncementPreferenceCityDistricts
    {
        private readonly List<Guid> _cityDistricts;

        public FlatForRentAnnouncementPreferenceCityDistricts(IEnumerable<Guid> cityDistricts)
        {
            if (cityDistricts is null)
                throw new FlatForRentAnnouncementPreferenceCityDistrictsNullException();

            var cityDistrictList = cityDistricts.ToList();

            if (cityDistrictList.Any(x => x == Guid.Empty || x == new Guid()))
                throw new FlatForRentAnnouncementPreferenceCityDistrictsNullException();

            var anyDuplicate = cityDistrictList.GroupBy(x => x).Any(g => g.Count() > 1);
            if (anyDuplicate)
                throw new FlatForRentAnnouncementPreferenceCityDistrictsDuplicateValuesException();

            _cityDistricts = new List<Guid>(cityDistrictList);
        }

        public static implicit operator List<Guid>(FlatForRentAnnouncementPreferenceCityDistricts cityDistricts)
        {
            return cityDistricts._cityDistricts;
        }
    }
}