using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Exceptions.AggregateExceptions
{
    [Serializable]
    public class UserRoomForRentAnnouncementPreferencesExceedAnnouncementPreferenceLimitException : DomainException
    {
        private const string ErrorMessage = "RoomForRentAnnouncementPreferences argument exceeded AnnouncementPreferenceLimit.";

        public UserRoomForRentAnnouncementPreferencesExceedAnnouncementPreferenceLimitException() : base(ErrorMessage)
        {
        }

        public UserRoomForRentAnnouncementPreferencesExceedAnnouncementPreferenceLimitException(string message) : base(message)
        {
        }

        public UserRoomForRentAnnouncementPreferencesExceedAnnouncementPreferenceLimitException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UserRoomForRentAnnouncementPreferencesExceedAnnouncementPreferenceLimitException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}