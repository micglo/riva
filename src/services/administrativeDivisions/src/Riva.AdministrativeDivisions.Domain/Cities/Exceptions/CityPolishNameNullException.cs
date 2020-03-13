using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.AdministrativeDivisions.Domain.Cities.Exceptions
{
    [Serializable]
    public class CityPolishNameNullException : DomainException
    {
        private const string ErrorMessage = "PolishName argument is required.";

        public CityPolishNameNullException() : base(ErrorMessage)
        {
        }

        public CityPolishNameNullException(string message) : base(message)
        {
        }

        public CityPolishNameNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CityPolishNameNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}