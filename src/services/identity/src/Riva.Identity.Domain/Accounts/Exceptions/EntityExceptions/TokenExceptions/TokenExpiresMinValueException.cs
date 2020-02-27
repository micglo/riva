using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Accounts.Exceptions.EntityExceptions.TokenExceptions
{
    [Serializable]
    public class TokenExpiresMinValueException : DomainException
    {
        private const string ErrorMessage = "Expires argument must be greater than Issued argument.";

        public TokenExpiresMinValueException() : base(ErrorMessage)
        {
        }

        public TokenExpiresMinValueException(string message) : base(message)
        {
        }

        public TokenExpiresMinValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public TokenExpiresMinValueException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}