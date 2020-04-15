using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Exceptions.EntityExceptions.FlatForRentAnnouncementPreferenceExceptions
{
    [Serializable]
    public class FlatForRentAnnouncementPreferenceIdNullException : DomainException
    {
        private const string ErrorMessage = "Id argument is required.";

        public FlatForRentAnnouncementPreferenceIdNullException() : base(ErrorMessage)
        {
        }

        public FlatForRentAnnouncementPreferenceIdNullException(string message) : base(message)
        {
        }

        public FlatForRentAnnouncementPreferenceIdNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public FlatForRentAnnouncementPreferenceIdNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}