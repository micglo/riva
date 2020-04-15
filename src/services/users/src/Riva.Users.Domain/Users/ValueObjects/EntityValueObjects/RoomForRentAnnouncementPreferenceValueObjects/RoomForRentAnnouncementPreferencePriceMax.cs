using Riva.Users.Domain.Users.Exceptions.EntityExceptions.RoomForRentAnnouncementPreferenceExceptions;

namespace Riva.Users.Domain.Users.ValueObjects.EntityValueObjects.RoomForRentAnnouncementPreferenceValueObjects
{
    public class RoomForRentAnnouncementPreferencePriceMax
    {
        private readonly decimal? _priceMax;
        private const int MinVale = 0;

        public RoomForRentAnnouncementPreferencePriceMax(decimal? priceMin, decimal? priceMax)
        {
            if (!priceMax.HasValue)
                _priceMax = null;
            else
            {
                if (priceMax.Value < MinVale)
                    throw new RoomForRentAnnouncementPreferencePriceMaxMinValueException(MinVale);
                if (priceMin.HasValue && priceMax.Value < priceMin.Value)
                    throw new RoomForRentAnnouncementPreferencePriceMaxLowerThanPriceMinException();
                _priceMax = priceMax;
            }
        }

        public static implicit operator decimal?(RoomForRentAnnouncementPreferencePriceMax priceMax)
        {
            return priceMax._priceMax;
        }
    }
}