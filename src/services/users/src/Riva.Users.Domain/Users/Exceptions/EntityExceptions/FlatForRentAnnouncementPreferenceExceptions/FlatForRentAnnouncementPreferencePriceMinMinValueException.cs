using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Exceptions.EntityExceptions.FlatForRentAnnouncementPreferenceExceptions
{
    [Serializable]
    public class FlatForRentAnnouncementPreferencePriceMinMinValueException : DomainException
    {
        private const string ErrorMessage = "PriceMin argument is invalid.";

        public FlatForRentAnnouncementPreferencePriceMinMinValueException() : base(ErrorMessage)
        {
        }

        public FlatForRentAnnouncementPreferencePriceMinMinValueException(int minValue) : base($"PriceMin argument min value is {minValue}.")
        {
        }

        public FlatForRentAnnouncementPreferencePriceMinMinValueException(string message) : base(message)
        {
        }

        public FlatForRentAnnouncementPreferencePriceMinMinValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public FlatForRentAnnouncementPreferencePriceMinMinValueException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}