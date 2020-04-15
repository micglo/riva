using System;
using System.Collections.Generic;
using System.Linq;
using Riva.Users.Domain.Users.Exceptions.EntityExceptions.RoomForRentAnnouncementPreferenceExceptions;

namespace Riva.Users.Domain.Users.ValueObjects.EntityValueObjects.RoomForRentAnnouncementPreferenceValueObjects
{
    public class RoomForRentAnnouncementPreferenceCityDistricts
    {
        private readonly List<Guid> _cityDistricts;

        public RoomForRentAnnouncementPreferenceCityDistricts(IEnumerable<Guid> cityDistricts)
        {
            if (cityDistricts is null)
                throw new RoomForRentAnnouncementPreferenceCityDistrictsNullException();

            var cityDistrictList = cityDistricts.ToList();

            if (cityDistrictList.Any(x => x == Guid.Empty || x == new Guid()))
                throw new RoomForRentAnnouncementPreferenceCityDistrictsNullException();

            var anyDuplicate = cityDistrictList.GroupBy(x => x).Any(g => g.Count() > 1);
            if (anyDuplicate)
                throw new RoomForRentAnnouncementPreferenceCityDistrictsDuplicateValuesException();

            _cityDistricts = new List<Guid>(cityDistrictList);
        }

        public static implicit operator List<Guid>(RoomForRentAnnouncementPreferenceCityDistricts cityDistricts)
        {
            return cityDistricts._cityDistricts;
        }
    }
}