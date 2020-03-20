using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Announcements.Domain.RoomForRentAnnouncements.Exceptions
{
    [Serializable]
    public class RoomForRentAnnouncementSourceUrlNullException : DomainException
    {
        private const string ErrorMessage = "SourceUrl argument is required.";

        public RoomForRentAnnouncementSourceUrlNullException() : base(ErrorMessage)
        {
        }

        public RoomForRentAnnouncementSourceUrlNullException(string message) : base(message)
        {
        }

        public RoomForRentAnnouncementSourceUrlNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RoomForRentAnnouncementSourceUrlNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}