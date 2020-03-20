using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Announcements.Domain.RoomForRentAnnouncements.Exceptions
{
    [Serializable]
    public class RoomForRentAnnouncementRoomTypeNullException : DomainException
    {
        private const string ErrorMessage = "RoomType argument is required.";

        public RoomForRentAnnouncementRoomTypeNullException() : base(ErrorMessage)
        {
        }

        public RoomForRentAnnouncementRoomTypeNullException(string message) : base(message)
        {
        }

        public RoomForRentAnnouncementRoomTypeNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RoomForRentAnnouncementRoomTypeNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}