using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Exceptions.EntityExceptions.FlatForRentAnnouncementPreferenceExceptions
{
    [Serializable]
    public class FlatForRentAnnouncementPreferencePriceMaxMinValueException : DomainException
    {
        private const string ErrorMessage = "PriceMax argument is invalid.";

        public FlatForRentAnnouncementPreferencePriceMaxMinValueException() : base(ErrorMessage)
        {
        }

        public FlatForRentAnnouncementPreferencePriceMaxMinValueException(int minValue) : base($"PriceMax argument min value is {minValue}.")
        {
        }

        public FlatForRentAnnouncementPreferencePriceMaxMinValueException(string message) : base(message)
        {
        }

        public FlatForRentAnnouncementPreferencePriceMaxMinValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public FlatForRentAnnouncementPreferencePriceMaxMinValueException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}