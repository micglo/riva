using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Announcements.Domain.FlatForRentAnnouncements.Exceptions
{
    [Serializable]
    public class FlatForRentAnnouncementTitleMaxLengthException : DomainException
    {
        private const string ErrorMessage = "Title argument max length is invalid.";

        public FlatForRentAnnouncementTitleMaxLengthException(int nameMaxLength) : base($"Title argument max length is {nameMaxLength}.")
        {
        }

        public FlatForRentAnnouncementTitleMaxLengthException() : base(ErrorMessage)
        {
        }

        public FlatForRentAnnouncementTitleMaxLengthException(string message) : base(message)
        {
        }

        public FlatForRentAnnouncementTitleMaxLengthException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public FlatForRentAnnouncementTitleMaxLengthException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}