using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Cities.Exceptions
{
    [Serializable]
    public class CityCityDistrictsInvalidsValueException : DomainException
    {
        private const string ErrorMessage = "CityDistricts argument is invalid.";

        public CityCityDistrictsInvalidsValueException() : base(ErrorMessage)
        {
        }

        public CityCityDistrictsInvalidsValueException(string message) : base(message)
        {
        }

        public CityCityDistrictsInvalidsValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CityCityDistrictsInvalidsValueException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}