using Riva.Users.Domain.Users.Exceptions.EntityExceptions.FlatForRentAnnouncementPreferenceExceptions;

namespace Riva.Users.Domain.Users.ValueObjects.EntityValueObjects.FlatForRentAnnouncementPreferenceValueObjects
{
    public class FlatForRentAnnouncementPreferenceRoomNumbersMax
    {
        private readonly int? _roomNumbersMax;
        private const int MinVale = 1;

        public FlatForRentAnnouncementPreferenceRoomNumbersMax(int? roomNumbersMin, int? roomNumbersMax)
        {
            if (!roomNumbersMax.HasValue)
                _roomNumbersMax = null;
            else
            {
                if (roomNumbersMax.Value < MinVale)
                    throw new FlatForRentAnnouncementPreferenceRoomNumbersMaxMinValueException(MinVale);
                if (roomNumbersMin.HasValue && roomNumbersMax.Value < roomNumbersMin.Value)
                    throw new FlatForRentAnnouncementPreferenceRoomNumbersMaxLowerThanRoomNumbersMinException();
                _roomNumbersMax = roomNumbersMax;
            }
        }

        public static implicit operator int?(FlatForRentAnnouncementPreferenceRoomNumbersMax roomNumbersMax)
        {
            return roomNumbersMax._roomNumbersMax;
        }
    }
}