using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Identity.Domain.PersistedGrants.Exceptions
{
    [Serializable]
    public class PersistedGrantSubjectIdNullException : DomainException
    {
        private const string ErrorMessage = "SubjectId argument is required.";

        public PersistedGrantSubjectIdNullException() : base(ErrorMessage)
        {
        }

        public PersistedGrantSubjectIdNullException(string message) : base(message)
        {
        }

        public PersistedGrantSubjectIdNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public PersistedGrantSubjectIdNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}