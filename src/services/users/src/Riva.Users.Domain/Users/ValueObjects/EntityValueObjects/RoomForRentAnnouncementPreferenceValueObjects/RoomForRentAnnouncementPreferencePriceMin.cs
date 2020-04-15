using Riva.Users.Domain.Users.Exceptions.EntityExceptions.RoomForRentAnnouncementPreferenceExceptions;

namespace Riva.Users.Domain.Users.ValueObjects.EntityValueObjects.RoomForRentAnnouncementPreferenceValueObjects
{
    public class RoomForRentAnnouncementPreferencePriceMin
    {
        private readonly decimal? _priceMin;
        private const int MinVale = 0;

        public RoomForRentAnnouncementPreferencePriceMin(decimal? priceMin)
        {
            if (!priceMin.HasValue)
                _priceMin = null;
            else
            {
                if (priceMin.Value < MinVale)
                    throw new RoomForRentAnnouncementPreferencePriceMinMinValueException(MinVale);
                _priceMin = priceMin;
            }
        }

        public static implicit operator decimal?(RoomForRentAnnouncementPreferencePriceMin priceMin)
        {
            return priceMin._priceMin;
        }
    }
}