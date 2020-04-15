using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Exceptions.EntityExceptions.FlatForRentAnnouncementPreferenceExceptions
{
    [Serializable]
    public class FlatForRentAnnouncementPreferenceCityIdNullException : DomainException
    {
        private const string ErrorMessage = "CityId argument is required.";

        public FlatForRentAnnouncementPreferenceCityIdNullException() : base(ErrorMessage)
        {
        }

        public FlatForRentAnnouncementPreferenceCityIdNullException(string message) : base(message)
        {
        }

        public FlatForRentAnnouncementPreferenceCityIdNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public FlatForRentAnnouncementPreferenceCityIdNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}