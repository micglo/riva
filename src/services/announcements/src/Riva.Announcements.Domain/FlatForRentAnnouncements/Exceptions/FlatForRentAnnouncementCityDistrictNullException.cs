using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Announcements.Domain.FlatForRentAnnouncements.Exceptions
{
    [Serializable]
    public class FlatForRentAnnouncementCityDistrictNullException : DomainException
    {
        private const string ErrorMessage = "CityDistrict argument is required.";

        public FlatForRentAnnouncementCityDistrictNullException() : base(ErrorMessage)
        {
        }

        public FlatForRentAnnouncementCityDistrictNullException(string message) : base(message)
        {
        }

        public FlatForRentAnnouncementCityDistrictNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public FlatForRentAnnouncementCityDistrictNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}