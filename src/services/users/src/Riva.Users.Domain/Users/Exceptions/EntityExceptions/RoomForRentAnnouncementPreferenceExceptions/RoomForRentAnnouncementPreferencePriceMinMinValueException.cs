using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Exceptions.EntityExceptions.RoomForRentAnnouncementPreferenceExceptions
{
    [Serializable]
    public class RoomForRentAnnouncementPreferencePriceMinMinValueException : DomainException
    {
        private const string ErrorMessage = "PriceMin argument is invalid.";

        public RoomForRentAnnouncementPreferencePriceMinMinValueException() : base(ErrorMessage)
        {
        }

        public RoomForRentAnnouncementPreferencePriceMinMinValueException(int minValue) : base($"PriceMin argument min value is {minValue}.")
        {
        }

        public RoomForRentAnnouncementPreferencePriceMinMinValueException(string message) : base(message)
        {
        }

        public RoomForRentAnnouncementPreferencePriceMinMinValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RoomForRentAnnouncementPreferencePriceMinMinValueException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}