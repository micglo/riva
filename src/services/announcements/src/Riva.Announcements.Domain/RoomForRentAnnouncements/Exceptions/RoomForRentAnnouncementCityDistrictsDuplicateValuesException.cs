using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Announcements.Domain.RoomForRentAnnouncements.Exceptions
{
    [Serializable]
    public class RoomForRentAnnouncementCityDistrictsDuplicateValuesException : DomainException
    {
        private const string ErrorMessage = "CityDistricts argument contains duplicate values.";

        public RoomForRentAnnouncementCityDistrictsDuplicateValuesException() : base(ErrorMessage)
        {
        }

        public RoomForRentAnnouncementCityDistrictsDuplicateValuesException(string message) : base(message)
        {
        }

        public RoomForRentAnnouncementCityDistrictsDuplicateValuesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RoomForRentAnnouncementCityDistrictsDuplicateValuesException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}