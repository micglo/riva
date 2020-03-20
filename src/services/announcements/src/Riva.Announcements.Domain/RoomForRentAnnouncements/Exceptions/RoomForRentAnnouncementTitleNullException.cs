using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Announcements.Domain.RoomForRentAnnouncements.Exceptions
{
    [Serializable]
    public class RoomForRentAnnouncementTitleNullException : DomainException
    {
        private const string ErrorMessage = "Title argument is required.";

        public RoomForRentAnnouncementTitleNullException() : base(ErrorMessage)
        {
        }

        public RoomForRentAnnouncementTitleNullException(string message) : base(message)
        {
        }

        public RoomForRentAnnouncementTitleNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RoomForRentAnnouncementTitleNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}