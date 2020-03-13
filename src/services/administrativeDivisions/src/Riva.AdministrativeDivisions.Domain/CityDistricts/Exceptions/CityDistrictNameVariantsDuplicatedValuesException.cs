using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.AdministrativeDivisions.Domain.CityDistricts.Exceptions
{
    [Serializable]
    public class CityDistrictNameVariantsDuplicatedValuesException : DomainException
    {
        private const string ErrorMessage = "NameVariants argument contains duplicate values.";

        public CityDistrictNameVariantsDuplicatedValuesException() : base(ErrorMessage)
        {
        }

        public CityDistrictNameVariantsDuplicatedValuesException(string message) : base(message)
        {
        }

        public CityDistrictNameVariantsDuplicatedValuesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CityDistrictNameVariantsDuplicatedValuesException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}