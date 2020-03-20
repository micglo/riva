using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Announcements.Domain.RoomForRentAnnouncements.Exceptions
{
    [Serializable]
    public class RoomForRentAnnouncementCityIdInvalidValueException : DomainException
    {
        private const string ErrorMessage = "CityId argument is invalid.";

        public RoomForRentAnnouncementCityIdInvalidValueException() : base(ErrorMessage)
        {
        }

        public RoomForRentAnnouncementCityIdInvalidValueException(string message) : base(message)
        {
        }

        public RoomForRentAnnouncementCityIdInvalidValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RoomForRentAnnouncementCityIdInvalidValueException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}