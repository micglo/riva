using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.AdministrativeDivisions.Domain.CityDistricts.Exceptions
{
    [Serializable]
    public class CityDistrictPolishNameNullException : DomainException
    {
        private const string ErrorMessage = "PolishName argument is required.";

        public CityDistrictPolishNameNullException() : base(ErrorMessage)
        {
        }

        public CityDistrictPolishNameNullException(string message) : base(message)
        {
        }

        public CityDistrictPolishNameNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CityDistrictPolishNameNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}