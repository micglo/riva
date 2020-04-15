using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Exceptions.AggregateExceptions
{
    [Serializable]
    public class UserEmailMaxLengthException : DomainException
    {
        private const string ErrorMessage = "Email argument max length is invalid.";

        public UserEmailMaxLengthException() : base(ErrorMessage)
        {
        }

        public UserEmailMaxLengthException(int emailMaxLength) : base($"Email argument max length is {emailMaxLength}.")
        {
        }

        public UserEmailMaxLengthException(string message) : base(message)
        {
        }

        public UserEmailMaxLengthException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UserEmailMaxLengthException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}