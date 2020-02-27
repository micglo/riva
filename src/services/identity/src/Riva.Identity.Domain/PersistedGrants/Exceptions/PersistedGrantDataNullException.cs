using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.PersistedGrants.Exceptions
{
    [Serializable]
    public class PersistedGrantDataNullException : DomainException
    {
        private const string ErrorMessage = "Data argument is required.";

        public PersistedGrantDataNullException() : base(ErrorMessage)
        {
        }

        public PersistedGrantDataNullException(string message) : base(message)
        {
        }

        public PersistedGrantDataNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public PersistedGrantDataNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}