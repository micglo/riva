using System;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Exceptions;

namespace Riva.Announcements.Domain.FlatForRentAnnouncements.ValueObjects
{
    public class FlatForRentAnnouncementCityDistrict
    {
        private readonly Guid _cityDistrict;

        public FlatForRentAnnouncementCityDistrict(Guid cityDistrict)
        {
            if (cityDistrict == Guid.Empty || cityDistrict == new Guid())
                throw new FlatForRentAnnouncementCityDistrictNullException();

            _cityDistrict = cityDistrict;
        }

        public static implicit operator Guid(FlatForRentAnnouncementCityDistrict cityDistrict)
        {
            return cityDistrict._cityDistrict;
        }
    }
}