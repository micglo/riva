using System;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Exceptions;

namespace Riva.Announcements.Domain.FlatForRentAnnouncements.ValueObjects
{
    public class FlatForRentAnnouncementCityId
    {
        private readonly Guid _cityId;

        public FlatForRentAnnouncementCityId(Guid cityId)
        {
            if (cityId == Guid.Empty || cityId == new Guid())
                throw new FlatForRentAnnouncementCityIdInvalidValueException();

            _cityId = cityId;
        }

        public static implicit operator Guid(FlatForRentAnnouncementCityId cityId)
        {
            return cityId._cityId;
        }
    }
}