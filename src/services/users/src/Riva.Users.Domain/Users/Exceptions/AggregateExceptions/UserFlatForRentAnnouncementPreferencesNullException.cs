using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Exceptions.AggregateExceptions
{
    [Serializable]
    public class UserFlatForRentAnnouncementPreferencesNullException : DomainException
    {
        private const string ErrorMessage = "FlatForRentAnnouncementPreferences argument is required.";

        public UserFlatForRentAnnouncementPreferencesNullException() : base(ErrorMessage)
        {
        }

        public UserFlatForRentAnnouncementPreferencesNullException(string message) : base(message)
        {
        }

        public UserFlatForRentAnnouncementPreferencesNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UserFlatForRentAnnouncementPreferencesNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}