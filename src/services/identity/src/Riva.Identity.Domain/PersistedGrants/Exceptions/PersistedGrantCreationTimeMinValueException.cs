using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.PersistedGrants.Exceptions
{
    [Serializable]
    public class PersistedGrantCreationTimeMinValueException : DomainException
    {
        private const string ErrorMessage = "CreationTime argument cannot be min value.";

        public PersistedGrantCreationTimeMinValueException() : base(ErrorMessage)
        {
        }

        public PersistedGrantCreationTimeMinValueException(string message) : base(message)
        {
        }

        public PersistedGrantCreationTimeMinValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public PersistedGrantCreationTimeMinValueException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}