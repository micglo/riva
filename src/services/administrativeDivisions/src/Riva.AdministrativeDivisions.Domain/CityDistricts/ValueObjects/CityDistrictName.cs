using Riva.AdministrativeDivisions.Domain.CityDistricts.Exceptions;

namespace Riva.AdministrativeDivisions.Domain.CityDistricts.ValueObjects
{
    public class CityDistrictName
    {
        private const int MaxLength = 256;
        private readonly string _name;

        public CityDistrictName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new CityDistrictNameNullException();
            if (name.Length > MaxLength)
                throw new CityDistrictNameMaxLengthException(MaxLength);

            _name = name;
        }

        public static implicit operator string(CityDistrictName name)
        {
            return name._name;
        }
    }
}