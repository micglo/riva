using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.AdministrativeDivisions.Domain.States.Exceptions
{
    [Serializable]
    public class StateNameMaxLengthException : DomainException
    {
        private const string ErrorMessage = "Name argument max length is invalid.";

        public StateNameMaxLengthException() : base(ErrorMessage)
        {
        }

        public StateNameMaxLengthException(int nameMaxLength) : base($"Name argument max length is {nameMaxLength}.")
        {
        }

        public StateNameMaxLengthException(string message) : base(message)
        {
        }

        public StateNameMaxLengthException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public StateNameMaxLengthException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}