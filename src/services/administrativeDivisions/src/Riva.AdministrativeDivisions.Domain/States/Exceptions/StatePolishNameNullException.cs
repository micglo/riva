using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.AdministrativeDivisions.Domain.States.Exceptions
{
    [Serializable]
    public class StatePolishNameNullException : DomainException
    {
        private const string ErrorMessage = "PolishName argument is required.";

        public StatePolishNameNullException() : base(ErrorMessage)
        {
        }

        public StatePolishNameNullException(string message) : base(message)
        {
        }

        public StatePolishNameNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public StatePolishNameNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}