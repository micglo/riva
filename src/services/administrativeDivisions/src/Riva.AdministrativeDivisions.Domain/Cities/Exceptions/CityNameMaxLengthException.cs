using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.AdministrativeDivisions.Domain.Cities.Exceptions
{
    [Serializable]
    public class CityNameMaxLengthException : DomainException
    {
        private const string ErrorMessage = "Name argument max length is invalid.";

        public CityNameMaxLengthException() : base(ErrorMessage)
        {
        }

        public CityNameMaxLengthException(int nameMaxLength) : base($"Name argument max length is {nameMaxLength}.")
        {
        }

        public CityNameMaxLengthException(string message) : base(message)
        {
        }

        public CityNameMaxLengthException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CityNameMaxLengthException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}