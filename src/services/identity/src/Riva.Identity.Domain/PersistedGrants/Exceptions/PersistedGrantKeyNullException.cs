using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.PersistedGrants.Exceptions
{
    [Serializable]
    public class PersistedGrantKeyNullException : DomainException
    {
        private const string ErrorMessage = "Key argument is required.";

        public PersistedGrantKeyNullException() : base(ErrorMessage)
        {
        }

        public PersistedGrantKeyNullException(string message) : base(message)
        {
        }

        public PersistedGrantKeyNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public PersistedGrantKeyNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}