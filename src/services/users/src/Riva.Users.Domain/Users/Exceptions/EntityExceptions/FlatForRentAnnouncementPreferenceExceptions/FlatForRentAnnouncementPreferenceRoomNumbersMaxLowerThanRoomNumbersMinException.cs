using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Exceptions.EntityExceptions.FlatForRentAnnouncementPreferenceExceptions
{
    [Serializable]
    public class FlatForRentAnnouncementPreferenceRoomNumbersMaxLowerThanRoomNumbersMinException : DomainException
    {
        private const string ErrorMessage = "RoomNumbersMax argument cannot be lower than RoomNumbersMin.";

        public FlatForRentAnnouncementPreferenceRoomNumbersMaxLowerThanRoomNumbersMinException() : base(ErrorMessage)
        {
        }

        public FlatForRentAnnouncementPreferenceRoomNumbersMaxLowerThanRoomNumbersMinException(string message) : base(message)
        {
        }

        public FlatForRentAnnouncementPreferenceRoomNumbersMaxLowerThanRoomNumbersMinException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public FlatForRentAnnouncementPreferenceRoomNumbersMaxLowerThanRoomNumbersMinException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}