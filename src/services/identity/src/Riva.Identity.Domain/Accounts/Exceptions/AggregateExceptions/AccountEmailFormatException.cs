using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Accounts.Exceptions.AggregateExceptions
{
    [Serializable]
    public class AccountEmailFormatException : DomainException
    {
        private const string ErrorMessage = "Email argument is not in the form required for an e-mail address.";

        public AccountEmailFormatException() : base(ErrorMessage)
        {
        }

        public AccountEmailFormatException(string message) : base(message)
        {
        }

        public AccountEmailFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AccountEmailFormatException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}