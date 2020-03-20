using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Announcements.Domain.FlatForRentAnnouncements.Exceptions
{
    [Serializable]
    public class FlatForRentAnnouncementCityDistrictsNullException : DomainException
    {
        private const string ErrorMessage = "CityDistricts argument is required.";

        public FlatForRentAnnouncementCityDistrictsNullException() : base(ErrorMessage)
        {
        }

        public FlatForRentAnnouncementCityDistrictsNullException(string message) : base(message)
        {
        }

        public FlatForRentAnnouncementCityDistrictsNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public FlatForRentAnnouncementCityDistrictsNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}