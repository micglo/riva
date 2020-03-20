using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Announcements.Domain.FlatForRentAnnouncements.Exceptions
{
    [Serializable]
    public class FlatForRentAnnouncementDescriptionNullException : DomainException
    {
        private const string ErrorMessage = "Description argument is required.";

        public FlatForRentAnnouncementDescriptionNullException() : base(ErrorMessage)
        {
        }

        public FlatForRentAnnouncementDescriptionNullException(string message) : base(message)
        {
        }

        public FlatForRentAnnouncementDescriptionNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public FlatForRentAnnouncementDescriptionNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}