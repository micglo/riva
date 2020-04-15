using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Exceptions.EntityExceptions.RoomForRentAnnouncementPreferenceExceptions
{
    [Serializable]
    public class RoomForRentAnnouncementPreferenceCityIdNullException : DomainException
    {
        private const string ErrorMessage = "CityId argument is required.";

        public RoomForRentAnnouncementPreferenceCityIdNullException() : base(ErrorMessage)
        {
        }

        public RoomForRentAnnouncementPreferenceCityIdNullException(string message) : base(message)
        {
        }

        public RoomForRentAnnouncementPreferenceCityIdNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RoomForRentAnnouncementPreferenceCityIdNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}