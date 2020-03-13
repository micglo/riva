using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.AdministrativeDivisions.Domain.CityDistricts.Exceptions
{
    [Serializable]
    public class CityDistrictPolishNameMaxLengthException : DomainException
    {
        private const string ErrorMessage = "PolishName argument max length is invalid.";

        public CityDistrictPolishNameMaxLengthException() : base(ErrorMessage)
        {
        }

        public CityDistrictPolishNameMaxLengthException(int nameMaxLength) : base($"PolishName argument max length is {nameMaxLength}.")
        {
        }

        public CityDistrictPolishNameMaxLengthException(string message) : base(message)
        {
        }

        public CityDistrictPolishNameMaxLengthException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CityDistrictPolishNameMaxLengthException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}