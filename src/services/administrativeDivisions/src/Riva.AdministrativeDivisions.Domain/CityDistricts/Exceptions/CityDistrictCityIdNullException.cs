using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.AdministrativeDivisions.Domain.CityDistricts.Exceptions
{
    [Serializable]
    public class CityDistrictCityIdNullException : DomainException
    {
        private const string ErrorMessage = "CityId argument is required.";

        public CityDistrictCityIdNullException() : base(ErrorMessage)
        {
        }

        public CityDistrictCityIdNullException(string message) : base(message)
        {
        }

        public CityDistrictCityIdNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CityDistrictCityIdNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}