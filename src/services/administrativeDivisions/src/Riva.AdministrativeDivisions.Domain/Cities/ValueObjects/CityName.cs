using Riva.AdministrativeDivisions.Domain.Cities.Exceptions;

namespace Riva.AdministrativeDivisions.Domain.Cities.ValueObjects
{
    public class CityName
    {
        private const int MaxLength = 256;
        private readonly string _name;

        public CityName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new CityNameNullException();
            if (name.Length > MaxLength)
                throw new CityNameMaxLengthException(MaxLength);

            _name = name;
        }

        public static implicit operator string(CityName name)
        {
            return name._name;
        }
    }
}