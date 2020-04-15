using System;
using Riva.Users.Domain.Users.Exceptions.EntityExceptions.RoomForRentAnnouncementPreferenceExceptions;

namespace Riva.Users.Domain.Users.ValueObjects.EntityValueObjects.RoomForRentAnnouncementPreferenceValueObjects
{
    public class RoomForRentAnnouncementPreferenceCityId
    {
        private readonly Guid _cityId;

        public RoomForRentAnnouncementPreferenceCityId(Guid cityId)
        {
            if (cityId == Guid.Empty || cityId == new Guid())
                throw new RoomForRentAnnouncementPreferenceCityIdNullException();

            _cityId = cityId;
        }

        public static implicit operator Guid(RoomForRentAnnouncementPreferenceCityId cityId)
        {
            return cityId._cityId;
        }
    }
}