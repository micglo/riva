using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Announcements.Domain.RoomForRentAnnouncements.Exceptions
{
    [Serializable]
    public class RoomForRentAnnouncementCityDistrictsInvalidValueException : DomainException
    {
        private const string ErrorMessage = "CityDistricts argument is invalid.";

        public RoomForRentAnnouncementCityDistrictsInvalidValueException() : base(ErrorMessage)
        {
        }

        public RoomForRentAnnouncementCityDistrictsInvalidValueException(string message) : base(message)
        {
        }

        public RoomForRentAnnouncementCityDistrictsInvalidValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RoomForRentAnnouncementCityDistrictsInvalidValueException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}