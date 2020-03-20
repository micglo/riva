using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Announcements.Domain.FlatForRentAnnouncements.Exceptions
{
    [Serializable]
    public class FlatForRentAnnouncementCityDistrictsInvalidValueException : DomainException
    {
        private const string ErrorMessage = "CityDistricts argument is invalid.";

        public FlatForRentAnnouncementCityDistrictsInvalidValueException() : base(ErrorMessage)
        {
        }

        public FlatForRentAnnouncementCityDistrictsInvalidValueException(string message) : base(message)
        {
        }

        public FlatForRentAnnouncementCityDistrictsInvalidValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public FlatForRentAnnouncementCityDistrictsInvalidValueException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}