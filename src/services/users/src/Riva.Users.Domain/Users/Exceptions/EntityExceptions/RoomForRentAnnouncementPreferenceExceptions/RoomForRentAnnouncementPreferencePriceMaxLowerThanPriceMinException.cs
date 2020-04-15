using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Exceptions.EntityExceptions.RoomForRentAnnouncementPreferenceExceptions
{
    [Serializable]
    public class RoomForRentAnnouncementPreferencePriceMaxLowerThanPriceMinException : DomainException
    {
        private const string ErrorMessage = "PriceMax argument cannot be lower than PriceMin.";

        public RoomForRentAnnouncementPreferencePriceMaxLowerThanPriceMinException() : base(ErrorMessage)
        {
        }

        public RoomForRentAnnouncementPreferencePriceMaxLowerThanPriceMinException(string message) : base(message)
        {
        }

        public RoomForRentAnnouncementPreferencePriceMaxLowerThanPriceMinException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RoomForRentAnnouncementPreferencePriceMaxLowerThanPriceMinException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}