using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.PersistedGrants.Exceptions
{
    [Serializable]
    public class PersistedGrantClientIdNullException : DomainException
    {
        private const string ErrorMessage = "ClientId argument is required.";

        public PersistedGrantClientIdNullException() : base(ErrorMessage)
        {
        }

        public PersistedGrantClientIdNullException(string message) : base(message)
        {
        }

        public PersistedGrantClientIdNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public PersistedGrantClientIdNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}