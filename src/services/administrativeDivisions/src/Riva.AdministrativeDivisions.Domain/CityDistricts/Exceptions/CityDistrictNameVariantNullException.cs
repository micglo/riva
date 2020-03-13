using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.AdministrativeDivisions.Domain.CityDistricts.Exceptions
{
    [Serializable]
    public class CityDistrictNameVariantNullException : DomainException
    {
        private const string ErrorMessage = "NameVariant argument is required.";

        public CityDistrictNameVariantNullException() : base(ErrorMessage)
        {
        }

        public CityDistrictNameVariantNullException(string message) : base(message)
        {
        }

        public CityDistrictNameVariantNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CityDistrictNameVariantNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}