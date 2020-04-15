using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Exceptions.AggregateExceptions
{
    [Serializable]
    public class UserAnnouncementPreferenceLimitMinValueException : DomainException
    {
        private const string ErrorMessage = "AnnouncementPreferenceLimit minimum value is invalid.";

        public UserAnnouncementPreferenceLimitMinValueException() : base(ErrorMessage)
        {
        }

        public UserAnnouncementPreferenceLimitMinValueException(int minValue) : base($"AnnouncementPreferenceLimit minimum value is {minValue}.")
        {
        }

        public UserAnnouncementPreferenceLimitMinValueException(string message) : base(message)
        {
        }

        public UserAnnouncementPreferenceLimitMinValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UserAnnouncementPreferenceLimitMinValueException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}