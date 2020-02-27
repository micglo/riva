using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.PersistedGrants.Exceptions
{
    [Serializable]
    public class PersistedGrantTypeNullException : DomainException
    {
        private const string ErrorMessage = "Type argument is required.";

        public PersistedGrantTypeNullException() : base(ErrorMessage)
        {
        }

        public PersistedGrantTypeNullException(string message) : base(message)
        {
        }

        public PersistedGrantTypeNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public PersistedGrantTypeNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}