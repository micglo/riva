using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Exceptions.EntityExceptions.RoomForRentAnnouncementPreferenceExceptions
{
    [Serializable]
    public class RoomForRentAnnouncementPreferenceCityDistrictsDuplicateValuesException : DomainException
    {
        private const string ErrorMessage = "CityDistricts argument contains duplicate values.";

        public RoomForRentAnnouncementPreferenceCityDistrictsDuplicateValuesException() : base(ErrorMessage)
        {
        }

        public RoomForRentAnnouncementPreferenceCityDistrictsDuplicateValuesException(string message) : base(message)
        {
        }

        public RoomForRentAnnouncementPreferenceCityDistrictsDuplicateValuesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RoomForRentAnnouncementPreferenceCityDistrictsDuplicateValuesException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}