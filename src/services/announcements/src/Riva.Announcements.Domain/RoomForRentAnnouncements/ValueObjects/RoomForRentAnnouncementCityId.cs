using System;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Exceptions;

namespace Riva.Announcements.Domain.RoomForRentAnnouncements.ValueObjects
{
    public class RoomForRentAnnouncementCityId
    {
        private readonly Guid _cityId;

        public RoomForRentAnnouncementCityId(Guid cityId)
        {
            if (cityId == Guid.Empty || cityId == new Guid())
                throw new RoomForRentAnnouncementCityIdInvalidValueException();

            _cityId = cityId;
        }

        public static implicit operator Guid(RoomForRentAnnouncementCityId cityId)
        {
            return cityId._cityId;
        }
    }
}