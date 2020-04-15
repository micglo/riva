using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Exceptions.EntityExceptions.RoomForRentAnnouncementPreferenceExceptions
{
    [Serializable]
    public class RoomForRentAnnouncementPreferenceIdNullException : DomainException
    {
        private const string ErrorMessage = "Id argument is required.";

        public RoomForRentAnnouncementPreferenceIdNullException() : base(ErrorMessage)
        {
        }

        public RoomForRentAnnouncementPreferenceIdNullException(string message) : base(message)
        {
        }

        public RoomForRentAnnouncementPreferenceIdNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RoomForRentAnnouncementPreferenceIdNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}