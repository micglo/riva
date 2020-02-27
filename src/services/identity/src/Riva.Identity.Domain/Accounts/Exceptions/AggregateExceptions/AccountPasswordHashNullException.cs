using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Accounts.Exceptions.AggregateExceptions
{
    [Serializable]
    public class AccountPasswordHashNullException : DomainException
    {
        private const string ErrorMessage = "PasswordHash argument is required.";

        public AccountPasswordHashNullException() : base(ErrorMessage)
        {
        }

        public AccountPasswordHashNullException(string message) : base(message)
        {
        }

        public AccountPasswordHashNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AccountPasswordHashNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}