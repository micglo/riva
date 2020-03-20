using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Announcements.Domain.FlatForRentAnnouncements.Exceptions
{
    [Serializable]
    public class FlatForRentAnnouncementSourceUrlNullException : DomainException
    {
        private const string ErrorMessage = "SourceUrl argument is required.";

        public FlatForRentAnnouncementSourceUrlNullException() : base(ErrorMessage)
        {
        }

        public FlatForRentAnnouncementSourceUrlNullException(string message) : base(message)
        {
        }

        public FlatForRentAnnouncementSourceUrlNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public FlatForRentAnnouncementSourceUrlNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}