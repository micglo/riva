using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.AdministrativeDivisions.Domain.CityDistricts.Exceptions
{
    [Serializable]
    public class CityDistrictNameMaxLengthException : DomainException
    {
        private const string ErrorMessage = "Name argument max length is invalid.";

        public CityDistrictNameMaxLengthException() : base(ErrorMessage)
        {
        }

        public CityDistrictNameMaxLengthException(int nameMaxLength) : base($"Name argument max length is {nameMaxLength}.")
        {
        }

        public CityDistrictNameMaxLengthException(string message) : base(message)
        {
        }

        public CityDistrictNameMaxLengthException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CityDistrictNameMaxLengthException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}