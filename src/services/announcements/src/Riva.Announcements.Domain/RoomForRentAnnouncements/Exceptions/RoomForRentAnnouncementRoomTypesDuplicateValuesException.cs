using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Announcements.Domain.RoomForRentAnnouncements.Exceptions
{
    [Serializable]
    public class RoomForRentAnnouncementRoomTypesDuplicateValuesException : DomainException
    {
        private const string ErrorMessage = "RoomTypes argument contains duplicate values.";

        public RoomForRentAnnouncementRoomTypesDuplicateValuesException() : base(ErrorMessage)
        {
        }

        public RoomForRentAnnouncementRoomTypesDuplicateValuesException(string message) : base(message)
        {
        }

        public RoomForRentAnnouncementRoomTypesDuplicateValuesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RoomForRentAnnouncementRoomTypesDuplicateValuesException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}