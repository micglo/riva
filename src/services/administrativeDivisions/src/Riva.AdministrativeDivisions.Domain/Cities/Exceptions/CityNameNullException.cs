using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.AdministrativeDivisions.Domain.Cities.Exceptions
{
    [Serializable]
    public class CityNameNullException : DomainException
    {
        private const string ErrorMessage = "Name argument is required.";

        public CityNameNullException() : base(ErrorMessage)
        {
        }

        public CityNameNullException(string message) : base(message)
        {
        }

        public CityNameNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CityNameNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}