using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Announcements.Domain.FlatForRentAnnouncements.Exceptions
{
    [Serializable]
    public class FlatForRentAnnouncementCityDistrictsDuplicateValuesException : DomainException
    {
        private const string ErrorMessage = "CityDistricts argument contains duplicate values.";

        public FlatForRentAnnouncementCityDistrictsDuplicateValuesException() : base(ErrorMessage)
        {
        }

        public FlatForRentAnnouncementCityDistrictsDuplicateValuesException(string message) : base(message)
        {
        }

        public FlatForRentAnnouncementCityDistrictsDuplicateValuesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public FlatForRentAnnouncementCityDistrictsDuplicateValuesException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}