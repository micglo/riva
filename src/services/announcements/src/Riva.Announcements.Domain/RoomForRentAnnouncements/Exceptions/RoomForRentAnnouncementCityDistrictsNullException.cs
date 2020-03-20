using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Announcements.Domain.RoomForRentAnnouncements.Exceptions
{
    [Serializable]
    public class RoomForRentAnnouncementCityDistrictsNullException : DomainException
    {
        private const string ErrorMessage = "CityDistricts argument is required.";

        public RoomForRentAnnouncementCityDistrictsNullException() : base(ErrorMessage)
        {
        }

        public RoomForRentAnnouncementCityDistrictsNullException(string message) : base(message)
        {
        }

        public RoomForRentAnnouncementCityDistrictsNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RoomForRentAnnouncementCityDistrictsNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}