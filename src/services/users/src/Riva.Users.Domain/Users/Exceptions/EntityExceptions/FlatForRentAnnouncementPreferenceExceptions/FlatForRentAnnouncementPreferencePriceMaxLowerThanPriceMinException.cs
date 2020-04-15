using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Exceptions.EntityExceptions.FlatForRentAnnouncementPreferenceExceptions
{
    [Serializable]
    public class FlatForRentAnnouncementPreferencePriceMaxLowerThanPriceMinException : DomainException
    {
        private const string ErrorMessage = "PriceMax argument cannot be lower than PriceMin.";

        public FlatForRentAnnouncementPreferencePriceMaxLowerThanPriceMinException() : base(ErrorMessage)
        {
        }

        public FlatForRentAnnouncementPreferencePriceMaxLowerThanPriceMinException(string message) : base(message)
        {
        }

        public FlatForRentAnnouncementPreferencePriceMaxLowerThanPriceMinException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public FlatForRentAnnouncementPreferencePriceMaxLowerThanPriceMinException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}