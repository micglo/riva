using System;
using System.Collections.Generic;
using System.Linq;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Exceptions;

namespace Riva.Announcements.Domain.RoomForRentAnnouncements.ValueObjects
{
    public class RoomForRentAnnouncementCityDistricts
    {
        private readonly List<Guid> _cityDistricts;

        public RoomForRentAnnouncementCityDistricts(IEnumerable<Guid> cityDistricts)
        {
            if (cityDistricts is null)
                throw new RoomForRentAnnouncementCityDistrictsNullException();

            var cityDistrictList = cityDistricts.ToList();

            if (cityDistrictList.Any(x => x == Guid.Empty || x == new Guid()))
                throw new RoomForRentAnnouncementCityDistrictsInvalidValueException();

            var anyDuplicate = cityDistrictList.GroupBy(x => x).Any(g => g.Count() > 1);
            if (anyDuplicate)
                throw new RoomForRentAnnouncementCityDistrictsDuplicateValuesException();

            _cityDistricts = new List<Guid>(cityDistrictList);
        }

        public static implicit operator List<Guid>(RoomForRentAnnouncementCityDistricts cityDistricts)
        {
            return cityDistricts._cityDistricts;
        }
    }
}