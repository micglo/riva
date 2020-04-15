using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Exceptions.EntityExceptions.RoomForRentAnnouncementPreferenceExceptions
{
    [Serializable]
    public class RoomForRentAnnouncementPreferencePriceMaxMinValueException : DomainException
    {
        private const string ErrorMessage = "PriceMax argument is invalid.";

        public RoomForRentAnnouncementPreferencePriceMaxMinValueException() : base(ErrorMessage)
        {
        }

        public RoomForRentAnnouncementPreferencePriceMaxMinValueException(int minValue) : base($"PriceMax argument min value is {minValue}.")
        {
        }

        public RoomForRentAnnouncementPreferencePriceMaxMinValueException(string message) : base(message)
        {
        }

        public RoomForRentAnnouncementPreferencePriceMaxMinValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RoomForRentAnnouncementPreferencePriceMaxMinValueException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}