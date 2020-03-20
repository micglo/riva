using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Announcements.Domain.FlatForRentAnnouncements.Exceptions
{
    [Serializable]
    public class FlatForRentAnnouncementTitleNullException : DomainException
    {
        private const string ErrorMessage = "Title argument is required.";

        public FlatForRentAnnouncementTitleNullException() : base(ErrorMessage)
        {
        }

        public FlatForRentAnnouncementTitleNullException(string message) : base(message)
        {
        }

        public FlatForRentAnnouncementTitleNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public FlatForRentAnnouncementTitleNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}