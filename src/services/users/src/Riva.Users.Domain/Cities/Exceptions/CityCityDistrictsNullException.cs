using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Cities.Exceptions
{
    [Serializable]
    public class CityCityDistrictsNullException : DomainException
    {
        private const string ErrorMessage = "CityDistricts argument is required.";

        public CityCityDistrictsNullException() : base(ErrorMessage)
        {
        }

        public CityCityDistrictsNullException(string message) : base(message)
        {
        }

        public CityCityDistrictsNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CityCityDistrictsNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}