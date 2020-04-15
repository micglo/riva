using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Exceptions.EntityExceptions.RoomForRentAnnouncementPreferenceExceptions
{
    [Serializable]
    public class RoomForRentAnnouncementPreferenceCityDistrictsNullException : DomainException
    {
        private const string ErrorMessage = "CityDistricts argument is required.";

        public RoomForRentAnnouncementPreferenceCityDistrictsNullException() : base(ErrorMessage)
        {
        }

        public RoomForRentAnnouncementPreferenceCityDistrictsNullException(string message) : base(message)
        {
        }

        public RoomForRentAnnouncementPreferenceCityDistrictsNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RoomForRentAnnouncementPreferenceCityDistrictsNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}