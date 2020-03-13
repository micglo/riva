using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.AdministrativeDivisions.Domain.CityDistricts.Exceptions
{
    [Serializable]
    public class CityDistrictNameVariantsInvalidValueException : DomainException
    {
        private const string ErrorMessage = "NameVariants argument is invalid.";

        public CityDistrictNameVariantsInvalidValueException() : base(ErrorMessage)
        {
        }

        public CityDistrictNameVariantsInvalidValueException(string message) : base(message)
        {
        }

        public CityDistrictNameVariantsInvalidValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CityDistrictNameVariantsInvalidValueException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}