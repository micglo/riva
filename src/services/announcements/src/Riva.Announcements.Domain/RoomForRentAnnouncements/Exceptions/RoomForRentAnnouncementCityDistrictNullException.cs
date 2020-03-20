using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Announcements.Domain.RoomForRentAnnouncements.Exceptions
{
    [Serializable]
    public class RoomForRentAnnouncementCityDistrictNullException : DomainException
    {
        private const string ErrorMessage = "CityDistrict argument is required.";

        public RoomForRentAnnouncementCityDistrictNullException() : base(ErrorMessage)
        {
        }

        public RoomForRentAnnouncementCityDistrictNullException(string message) : base(message)
        {
        }

        public RoomForRentAnnouncementCityDistrictNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RoomForRentAnnouncementCityDistrictNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}