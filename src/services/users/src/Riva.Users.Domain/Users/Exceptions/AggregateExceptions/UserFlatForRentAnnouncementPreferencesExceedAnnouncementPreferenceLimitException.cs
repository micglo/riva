using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Exceptions.AggregateExceptions
{
    [Serializable]
    public class UserFlatForRentAnnouncementPreferencesExceedAnnouncementPreferenceLimitException : DomainException
    {
        private const string ErrorMessage = "FlatForRentAnnouncementPreferences argument exceeded AnnouncementPreferenceLimit.";

        public UserFlatForRentAnnouncementPreferencesExceedAnnouncementPreferenceLimitException() : base(ErrorMessage)
        {
        }

        public UserFlatForRentAnnouncementPreferencesExceedAnnouncementPreferenceLimitException(string message) : base(message)
        {
        }

        public UserFlatForRentAnnouncementPreferencesExceedAnnouncementPreferenceLimitException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UserFlatForRentAnnouncementPreferencesExceedAnnouncementPreferenceLimitException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}