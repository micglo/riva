using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.PersistedGrants.Exceptions
{
    [Serializable]
    public class PersistedGrantExpirationMinValueException : DomainException
    {
        private const string ErrorMessage = "Expiration argument cannot be earlier than CreationTime.";

        public PersistedGrantExpirationMinValueException() : base(ErrorMessage)
        {
        }

        public PersistedGrantExpirationMinValueException(string message) : base(message)
        {
        }

        public PersistedGrantExpirationMinValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public PersistedGrantExpirationMinValueException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}