using Riva.Users.Domain.Users.Exceptions.EntityExceptions.FlatForRentAnnouncementPreferenceExceptions;

namespace Riva.Users.Domain.Users.ValueObjects.EntityValueObjects.FlatForRentAnnouncementPreferenceValueObjects
{
    public class FlatForRentAnnouncementPreferencePriceMin
    {
        private readonly decimal? _priceMin;
        private const int MinVale = 0;

        public FlatForRentAnnouncementPreferencePriceMin(decimal? priceMin)
        {
            if (!priceMin.HasValue)
                _priceMin = null;
            else
            {
                if (priceMin.Value < MinVale)
                    throw new FlatForRentAnnouncementPreferencePriceMinMinValueException(MinVale);
                _priceMin = priceMin;
            }
        }

        public static implicit operator decimal?(FlatForRentAnnouncementPreferencePriceMin priceMin)
        {
            return priceMin._priceMin;
        }
    }
}