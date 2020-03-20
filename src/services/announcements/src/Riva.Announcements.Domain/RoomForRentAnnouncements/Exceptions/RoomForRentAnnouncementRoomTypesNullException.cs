using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Announcements.Domain.RoomForRentAnnouncements.Exceptions
{
    [Serializable]
    public class RoomForRentAnnouncementRoomTypesNullException : DomainException
    {
        private const string ErrorMessage = "RoomTypes argument is required.";

        public RoomForRentAnnouncementRoomTypesNullException() : base(ErrorMessage)
        {
        }

        public RoomForRentAnnouncementRoomTypesNullException(string message) : base(message)
        {
        }

        public RoomForRentAnnouncementRoomTypesNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RoomForRentAnnouncementRoomTypesNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}