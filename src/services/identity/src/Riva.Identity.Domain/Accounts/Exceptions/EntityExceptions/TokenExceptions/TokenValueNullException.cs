using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Accounts.Exceptions.EntityExceptions.TokenExceptions
{
    [Serializable]
    public class TokenValueNullException : DomainException
    {
        private const string ErrorMessage = "Value argument is required.";

        public TokenValueNullException() : base(ErrorMessage)
        {
        }

        public TokenValueNullException(string message) : base(message)
        {
        }

        public TokenValueNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public TokenValueNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}