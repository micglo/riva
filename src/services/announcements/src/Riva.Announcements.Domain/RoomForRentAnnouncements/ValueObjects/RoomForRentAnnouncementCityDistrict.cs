using System;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Exceptions;

namespace Riva.Announcements.Domain.RoomForRentAnnouncements.ValueObjects
{
    public class RoomForRentAnnouncementCityDistrict
    {
        private readonly Guid _cityDistrict;

        public RoomForRentAnnouncementCityDistrict(Guid cityDistrict)
        {
            if (cityDistrict == Guid.Empty || cityDistrict == new Guid())
                throw new RoomForRentAnnouncementCityDistrictNullException();

            _cityDistrict = cityDistrict;
        }

        public static implicit operator Guid(RoomForRentAnnouncementCityDistrict cityDistrict)
        {
            return cityDistrict._cityDistrict;
        }
    }
}