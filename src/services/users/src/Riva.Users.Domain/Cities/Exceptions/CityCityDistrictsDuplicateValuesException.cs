using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Cities.Exceptions
{
    [Serializable]
    public class CityCityDistrictsDuplicateValuesException : DomainException
    {
        private const string ErrorMessage = "CityDistricts argument contains duplicate values.";

        public CityCityDistrictsDuplicateValuesException() : base(ErrorMessage)
        {
        }

        public CityCityDistrictsDuplicateValuesException(string message) : base(message)
        {
        }

        public CityCityDistrictsDuplicateValuesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CityCityDistrictsDuplicateValuesException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}