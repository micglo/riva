using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Accounts.Exceptions.EntityExceptions.TokenExceptions
{
    [Serializable]
    public class TokenTypeNullException : DomainException
    {
        private const string ErrorMessage = "TokenType argument is required.";

        public TokenTypeNullException() : base(ErrorMessage)
        {
        }

        public TokenTypeNullException(string message) : base(message)
        {
        }

        public TokenTypeNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public TokenTypeNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}