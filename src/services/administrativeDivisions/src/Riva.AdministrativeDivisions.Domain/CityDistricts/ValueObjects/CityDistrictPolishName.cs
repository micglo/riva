using Riva.AdministrativeDivisions.Domain.CityDistricts.Exceptions;

namespace Riva.AdministrativeDivisions.Domain.CityDistricts.ValueObjects
{
    public class CityDistrictPolishName
    {
        private const int MaxLength = 256;
        private readonly string _polishName;

        public CityDistrictPolishName(string polishName)
        {
            if (string.IsNullOrWhiteSpace(polishName))
                throw new CityDistrictPolishNameNullException();
            if (polishName.Length > MaxLength)
                throw new CityDistrictPolishNameMaxLengthException(MaxLength);

            _polishName = polishName;
        }

        public static implicit operator string(CityDistrictPolishName polishName)
        {
            return polishName._polishName;
        }
    }
}