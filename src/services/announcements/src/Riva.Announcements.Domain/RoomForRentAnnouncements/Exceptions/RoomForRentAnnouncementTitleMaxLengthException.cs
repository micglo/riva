using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Announcements.Domain.RoomForRentAnnouncements.Exceptions
{
    [Serializable]
    public class RoomForRentAnnouncementTitleMaxLengthException : DomainException
    {
        private const string ErrorMessage = "Title argument max length is invalid.";

        public RoomForRentAnnouncementTitleMaxLengthException(int nameMaxLength) : base($"Title argument max length is {nameMaxLength}.")
        {
        }

        public RoomForRentAnnouncementTitleMaxLengthException() : base(ErrorMessage)
        {
        }

        public RoomForRentAnnouncementTitleMaxLengthException(string message) : base(message)
        {
        }

        public RoomForRentAnnouncementTitleMaxLengthException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RoomForRentAnnouncementTitleMaxLengthException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}