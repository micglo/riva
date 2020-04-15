using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Exceptions.AggregateExceptions
{
    [Serializable]
    public class UserRoomForRentAnnouncementPreferencesNullException : DomainException
    {
        private const string ErrorMessage = "RoomForRentAnnouncementPreferences argument is required.";

        public UserRoomForRentAnnouncementPreferencesNullException() : base(ErrorMessage)
        {
        }

        public UserRoomForRentAnnouncementPreferencesNullException(string message) : base(message)
        {
        }

        public UserRoomForRentAnnouncementPreferencesNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UserRoomForRentAnnouncementPreferencesNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}