using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Exceptions.AggregateExceptions
{
    [Serializable]
    public class UserRoomForRentAnnouncementPreferencesInvalidValueException : DomainException
    {
        private const string ErrorMessage = "RoomForRentAnnouncementPreferences argument is invalid.";

        public UserRoomForRentAnnouncementPreferencesInvalidValueException() : base(ErrorMessage)
        {
        }

        public UserRoomForRentAnnouncementPreferencesInvalidValueException(string message) : base(message)
        {
        }

        public UserRoomForRentAnnouncementPreferencesInvalidValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UserRoomForRentAnnouncementPreferencesInvalidValueException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}