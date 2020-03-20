using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Announcements.Domain.FlatForRentAnnouncements.Exceptions
{
    [Serializable]
    public class FlatForRentAnnouncementCityIdInvalidValueException : DomainException
    {
        private const string ErrorMessage = "CityId argument is invalid.";

        public FlatForRentAnnouncementCityIdInvalidValueException() : base(ErrorMessage)
        {
        }

        public FlatForRentAnnouncementCityIdInvalidValueException(string message) : base(message)
        {
        }

        public FlatForRentAnnouncementCityIdInvalidValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public FlatForRentAnnouncementCityIdInvalidValueException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}