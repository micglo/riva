using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Exceptions.EntityExceptions.FlatForRentAnnouncementPreferenceExceptions
{
    [Serializable]
    public class FlatForRentAnnouncementPreferenceCityDistrictsNullException : DomainException
    {
        private const string ErrorMessage = "CityDistricts argument is required.";

        public FlatForRentAnnouncementPreferenceCityDistrictsNullException() : base(ErrorMessage)
        {
        }

        public FlatForRentAnnouncementPreferenceCityDistrictsNullException(string message) : base(message)
        {
        }

        public FlatForRentAnnouncementPreferenceCityDistrictsNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public FlatForRentAnnouncementPreferenceCityDistrictsNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}