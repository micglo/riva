using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Announcements.Domain.RoomForRentAnnouncements.Exceptions
{
    [Serializable]
    public class RoomForRentAnnouncementDescriptionNullException : DomainException
    {
        private const string ErrorMessage = "Description argument is required.";

        public RoomForRentAnnouncementDescriptionNullException() : base(ErrorMessage)
        {
        }

        public RoomForRentAnnouncementDescriptionNullException(string message) : base(message)
        {
        }

        public RoomForRentAnnouncementDescriptionNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RoomForRentAnnouncementDescriptionNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}