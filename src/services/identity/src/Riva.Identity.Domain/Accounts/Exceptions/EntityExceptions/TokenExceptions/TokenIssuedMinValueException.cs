using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.Accounts.Exceptions.EntityExceptions.TokenExceptions
{
    [Serializable]
    public class TokenIssuedMinValueException : DomainException
    {
        private const string ErrorMessage = "Issued argument cannot be min value.";

        public TokenIssuedMinValueException() : base(ErrorMessage)
        {
        }

        public TokenIssuedMinValueException(string message) : base(message)
        {
        }

        public TokenIssuedMinValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public TokenIssuedMinValueException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}