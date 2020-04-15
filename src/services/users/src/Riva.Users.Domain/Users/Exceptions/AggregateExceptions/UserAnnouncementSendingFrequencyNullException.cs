using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Exceptions.AggregateExceptions
{
    [Serializable]
    public class UserAnnouncementSendingFrequencyNullException : DomainException
    {
        private const string ErrorMessage = "AnnouncementSendingFrequency argument is required.";

        public UserAnnouncementSendingFrequencyNullException() : base(ErrorMessage)
        {
        }

        public UserAnnouncementSendingFrequencyNullException(string message) : base(message)
        {
        }

        public UserAnnouncementSendingFrequencyNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UserAnnouncementSendingFrequencyNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}