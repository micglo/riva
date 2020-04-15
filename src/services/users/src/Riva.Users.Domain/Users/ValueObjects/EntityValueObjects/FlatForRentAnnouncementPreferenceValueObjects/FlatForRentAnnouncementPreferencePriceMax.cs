using Riva.Users.Domain.Users.Exceptions.EntityExceptions.FlatForRentAnnouncementPreferenceExceptions;

namespace Riva.Users.Domain.Users.ValueObjects.EntityValueObjects.FlatForRentAnnouncementPreferenceValueObjects
{
    public class FlatForRentAnnouncementPreferencePriceMax
    {
        private readonly decimal? _priceMax;
        private const int MinVale = 0;

        public FlatForRentAnnouncementPreferencePriceMax(decimal? priceMin, decimal? priceMax)
        {
            if (!priceMax.HasValue)
                _priceMax = null;
            else
            {
                if (priceMax.Value < MinVale)
                    throw new FlatForRentAnnouncementPreferencePriceMaxMinValueException(MinVale);
                if (priceMin.HasValue && priceMax.Value < priceMin.Value)
                    throw new FlatForRentAnnouncementPreferencePriceMaxLowerThanPriceMinException();
                _priceMax = priceMax;
            }
        }

        public static implicit operator decimal?(FlatForRentAnnouncementPreferencePriceMax priceMax)
        {
            return priceMax._priceMax;
        }
    }
}