using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Exceptions.AggregateExceptions
{
    [Serializable]
    public class UserFlatForRentAnnouncementPreferencesInvalidValueException : DomainException
    {
        private const string ErrorMessage = "FlatForRentAnnouncementPreferences argument is invalid.";

        public UserFlatForRentAnnouncementPreferencesInvalidValueException() : base(ErrorMessage)
        {
        }

        public UserFlatForRentAnnouncementPreferencesInvalidValueException(string message) : base(message)
        {
        }

        public UserFlatForRentAnnouncementPreferencesInvalidValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UserFlatForRentAnnouncementPreferencesInvalidValueException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}