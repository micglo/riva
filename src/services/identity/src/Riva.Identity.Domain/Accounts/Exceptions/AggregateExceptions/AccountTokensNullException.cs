using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Accounts.Exceptions.AggregateExceptions
{
    [Serializable]
    public class AccountTokensNullException : DomainException
    {
        private const string ErrorMessage = "Tokens argument is required.";

        public AccountTokensNullException() : base(ErrorMessage)
        {
        }

        public AccountTokensNullException(string message) : base(message)
        {
        }

        public AccountTokensNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AccountTokensNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}