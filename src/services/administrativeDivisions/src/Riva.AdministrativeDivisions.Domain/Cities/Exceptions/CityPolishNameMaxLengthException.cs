using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.AdministrativeDivisions.Domain.Cities.Exceptions
{
    [Serializable]
    public class CityPolishNameMaxLengthException : DomainException
    {
        private const string ErrorMessage = "PolishName argument max length is invalid.";

        public CityPolishNameMaxLengthException() : base(ErrorMessage)
        {
        }

        public CityPolishNameMaxLengthException(int nameMaxLength) : base($"PolishName argument max length is {nameMaxLength}.")
        {
        }

        public CityPolishNameMaxLengthException(string message) : base(message)
        {
        }

        public CityPolishNameMaxLengthException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CityPolishNameMaxLengthException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}