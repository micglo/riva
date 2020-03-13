using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.AdministrativeDivisions.Domain.States.Exceptions
{
    [Serializable]
    public class StateNameNullException : DomainException
    {
        private const string ErrorMessage = "Name argument is required.";

        public StateNameNullException() : base(ErrorMessage)
        {
        }

        public StateNameNullException(string message) : base(message)
        {
        }

        public StateNameNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public StateNameNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}