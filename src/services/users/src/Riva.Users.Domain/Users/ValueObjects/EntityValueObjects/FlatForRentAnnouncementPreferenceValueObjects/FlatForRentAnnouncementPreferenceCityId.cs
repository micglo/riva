using System;
using Riva.Users.Domain.Users.Exceptions.EntityExceptions.FlatForRentAnnouncementPreferenceExceptions;

namespace Riva.Users.Domain.Users.ValueObjects.EntityValueObjects.FlatForRentAnnouncementPreferenceValueObjects
{
    public class FlatForRentAnnouncementPreferenceCityId
    {
        private readonly Guid _cityId;

        public FlatForRentAnnouncementPreferenceCityId(Guid cityId)
        {
            if (cityId == Guid.Empty || cityId == new Guid())
                throw new FlatForRentAnnouncementPreferenceCityIdNullException();

            _cityId = cityId;
        }

        public static implicit operator Guid(FlatForRentAnnouncementPreferenceCityId cityId)
        {
            return cityId._cityId;
        }
    }
}