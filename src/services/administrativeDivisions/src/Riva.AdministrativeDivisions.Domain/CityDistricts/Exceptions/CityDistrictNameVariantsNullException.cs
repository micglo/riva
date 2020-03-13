using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.AdministrativeDivisions.Domain.CityDistricts.Exceptions
{
    [Serializable]
    public class CityDistrictNameVariantsNullException : DomainException
    {
        private const string ErrorMessage = "NameVariants argument is required.";

        public CityDistrictNameVariantsNullException() : base(ErrorMessage)
        {
        }

        public CityDistrictNameVariantsNullException(string message) : base(message)
        {
        }

        public CityDistrictNameVariantsNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CityDistrictNameVariantsNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}