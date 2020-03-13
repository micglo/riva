using Riva.AdministrativeDivisions.Domain.Cities.Exceptions;

namespace Riva.AdministrativeDivisions.Domain.Cities.ValueObjects
{
    public class CityPolishName
    {
        private const int MaxLength = 256;
        private readonly string _polishName;

        public CityPolishName(string polishName)
        {
            if (string.IsNullOrWhiteSpace(polishName))
                throw new CityPolishNameNullException();
            if (polishName.Length > MaxLength)
                throw new CityPolishNameMaxLengthException(MaxLength);

            _polishName = polishName;
        }

        public static implicit operator string(CityPolishName polishName)
        {
            return polishName._polishName;
        }
    }
}