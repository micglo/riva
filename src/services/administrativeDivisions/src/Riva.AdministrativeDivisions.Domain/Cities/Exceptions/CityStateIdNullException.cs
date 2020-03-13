using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.AdministrativeDivisions.Domain.Cities.Exceptions
{
    [Serializable]
    public class CityStateIdNullException : DomainException
    {
        private const string ErrorMessage = "StateId argument is required.";

        public CityStateIdNullException() : base(ErrorMessage)
        {
        }

        public CityStateIdNullException(string message) : base(message)
        {
        }

        public CityStateIdNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CityStateIdNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}