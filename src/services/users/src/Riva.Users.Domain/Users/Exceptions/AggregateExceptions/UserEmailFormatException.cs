using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Exceptions.AggregateExceptions
{
    [Serializable]
    public class UserEmailFormatException : DomainException
    {
        private const string ErrorMessage = "Email argument is not in the form required for an e-mail address.";

        public UserEmailFormatException() : base(ErrorMessage)
        {
        }

        public UserEmailFormatException(string message) : base(message)
        {
        }

        public UserEmailFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UserEmailFormatException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}