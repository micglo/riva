using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Exceptions.EntityExceptions.FlatForRentAnnouncementPreferenceExceptions
{
    [Serializable]
    public class FlatForRentAnnouncementPreferenceCityDistrictsDuplicateValuesException : DomainException
    {
        private const string ErrorMessage = "CityDistricts argument contains duplicate values.";

        public FlatForRentAnnouncementPreferenceCityDistrictsDuplicateValuesException() : base(ErrorMessage)
        {
        }

        public FlatForRentAnnouncementPreferenceCityDistrictsDuplicateValuesException(string message) : base(message)
        {
        }

        public FlatForRentAnnouncementPreferenceCityDistrictsDuplicateValuesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public FlatForRentAnnouncementPreferenceCityDistrictsDuplicateValuesException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}