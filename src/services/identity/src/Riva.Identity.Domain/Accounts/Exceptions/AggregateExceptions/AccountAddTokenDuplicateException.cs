using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Accounts.Exceptions.AggregateExceptions
{
    [Serializable]
    public class AccountAddTokenDuplicateException : DomainException
    {
        private const string ErrorMessage = "Account already have this type of token.";

        public AccountAddTokenDuplicateException() : base(ErrorMessage)
        {
        }

        public AccountAddTokenDuplicateException(string message) : base(message)
        {
        }

        public AccountAddTokenDuplicateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AccountAddTokenDuplicateException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}