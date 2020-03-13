using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.AdministrativeDivisions.Domain.States.Exceptions
{
    [Serializable]
    public class StatePolishNameMaxLengthException : DomainException
    {
        private const string ErrorMessage = "PolishName argument max length is invalid.";

        public StatePolishNameMaxLengthException() : base(ErrorMessage)
        {
        }

        public StatePolishNameMaxLengthException(int nameMaxLength) : base($"PolishName argument max length is {nameMaxLength}.")
        {
        }

        public StatePolishNameMaxLengthException(string message) : base(message)
        {
        }

        public StatePolishNameMaxLengthException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public StatePolishNameMaxLengthException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}