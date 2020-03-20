using System;
using System.Collections.Generic;
using System.Linq;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Exceptions;

namespace Riva.Announcements.Domain.FlatForRentAnnouncements.ValueObjects
{
    public class FlatForRentAnnouncementCityDistricts
    {
        private readonly List<Guid> _cityDistricts;

        public FlatForRentAnnouncementCityDistricts(IEnumerable<Guid> cityDistricts)
        {
            if (cityDistricts is null)
                throw new FlatForRentAnnouncementCityDistrictsNullException();

            var cityDistrictList = cityDistricts.ToList();

            if (cityDistrictList.Any(x => x == Guid.Empty || x == new Guid()))
                throw new FlatForRentAnnouncementCityDistrictsInvalidValueException();

            var anyDuplicate = cityDistrictList.GroupBy(x => x).Any(g => g.Count() > 1);
            if (anyDuplicate)
                throw new FlatForRentAnnouncementCityDistrictsDuplicateValuesException();

            _cityDistricts = new List<Guid>(cityDistrictList);
        }

        public static implicit operator List<Guid>(FlatForRentAnnouncementCityDistricts cityDistricts)
        {
            return cityDistricts._cityDistricts;
        }
    }
}