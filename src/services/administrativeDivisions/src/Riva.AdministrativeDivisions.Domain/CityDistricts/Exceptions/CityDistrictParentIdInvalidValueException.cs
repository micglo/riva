using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.AdministrativeDivisions.Domain.CityDistricts.Exceptions
{
    [Serializable]
    public class CityDistrictParentIdInvalidValueException : DomainException
    {
        private const string ErrorMessage = "ParentId argument is invalid.";

        public CityDistrictParentIdInvalidValueException() : base(ErrorMessage)
        {
        }

        public CityDistrictParentIdInvalidValueException(string message) : base(message)
        {
        }

        public CityDistrictParentIdInvalidValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CityDistrictParentIdInvalidValueException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}