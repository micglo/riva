using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Exceptions.EntityExceptions.FlatForRentAnnouncementPreferenceExceptions
{
    [Serializable]
    public class FlatForRentAnnouncementPreferenceRoomNumbersMaxMinValueException : DomainException
    {
        private const string ErrorMessage = "RoomNumbersMax argument is invalid.";

        public FlatForRentAnnouncementPreferenceRoomNumbersMaxMinValueException() : base(ErrorMessage)
        {
        }

        public FlatForRentAnnouncementPreferenceRoomNumbersMaxMinValueException(int minValue) : base($"RoomNumbersMax argument min value is {minValue}.")
        {
        }

        public FlatForRentAnnouncementPreferenceRoomNumbersMaxMinValueException(string message) : base(message)
        {
        }

        public FlatForRentAnnouncementPreferenceRoomNumbersMaxMinValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public FlatForRentAnnouncementPreferenceRoomNumbersMaxMinValueException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}