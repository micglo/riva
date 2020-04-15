using System;
using System.Runtime.Serialization;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Exceptions.EntityExceptions.FlatForRentAnnouncementPreferenceExceptions
{
    [Serializable]
    public class FlatForRentAnnouncementPreferenceRoomNumbersMinMinValueException : DomainException
    {
        private const string ErrorMessage = "RoomNumbersMin argument is invalid.";

        public FlatForRentAnnouncementPreferenceRoomNumbersMinMinValueException() : base(ErrorMessage)
        {
        }

        public FlatForRentAnnouncementPreferenceRoomNumbersMinMinValueException(int minValue) : base($"RoomNumbersMin argument min value is {minValue}.")
        {
        }

        public FlatForRentAnnouncementPreferenceRoomNumbersMinMinValueException(string message) : base(message)
        {
        }

        public FlatForRentAnnouncementPreferenceRoomNumbersMinMinValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public FlatForRentAnnouncementPreferenceRoomNumbersMinMinValueException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}