using Riva.Users.Domain.Users.Exceptions.EntityExceptions.FlatForRentAnnouncementPreferenceExceptions;

namespace Riva.Users.Domain.Users.ValueObjects.EntityValueObjects.FlatForRentAnnouncementPreferenceValueObjects
{
    public class FlatForRentAnnouncementPreferenceRoomNumbersMin
    {
        private readonly int? _roomNumbersMin;
        private const int MinVale = 1;

        public FlatForRentAnnouncementPreferenceRoomNumbersMin(int? roomNumbersMin)
        {
            if (!roomNumbersMin.HasValue)
                _roomNumbersMin = null;
            else
            {
                if (roomNumbersMin.Value < MinVale)
                    throw new FlatForRentAnnouncementPreferenceRoomNumbersMinMinValueException(MinVale);
                _roomNumbersMin = roomNumbersMin;
            }
        }

        public static implicit operator int?(FlatForRentAnnouncementPreferenceRoomNumbersMin roomNumbersMin)
        {
            return roomNumbersMin._roomNumbersMin;
        }
    }
}