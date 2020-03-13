using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.AdministrativeDivisions.Domain.CityDistricts.Exceptions
{
    [Serializable]
    public class CityDistrictNameNullException : DomainException
    {
        private const string ErrorMessage = "Name argument is required.";

        public CityDistrictNameNullException() : base(ErrorMessage)
        {
        }

        public CityDistrictNameNullException(string message) : base(message)
        {
        }

        public CityDistrictNameNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CityDistrictNameNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}