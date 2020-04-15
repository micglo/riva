using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Exceptions.AggregateExceptions
{
    [Serializable]
    public class UserEmailNullException : DomainException
    {
        private const string ErrorMessage = "Email argument is required.";

        public UserEmailNullException() : base(ErrorMessage)
        {
        }

        public UserEmailNullException(string message) : base(message)
        {
        }

        public UserEmailNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UserEmailNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}