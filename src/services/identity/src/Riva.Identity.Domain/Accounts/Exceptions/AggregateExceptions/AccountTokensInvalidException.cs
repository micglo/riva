using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Accounts.Exceptions.AggregateExceptions
{
    [Serializable]
    public class AccountTokensInvalidException : DomainException
    {
        private const string ErrorMessage = "Tokens argument is invalid.";

        public AccountTokensInvalidException() : base(ErrorMessage)
        {
        }

        public AccountTokensInvalidException(string message) : base(message)
        {
        }

        public AccountTokensInvalidException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AccountTokensInvalidException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}