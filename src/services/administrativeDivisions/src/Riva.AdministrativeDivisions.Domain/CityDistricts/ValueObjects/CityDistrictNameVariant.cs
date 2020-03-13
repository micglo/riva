using Riva.AdministrativeDivisions.Domain.CityDistricts.Exceptions;

namespace Riva.AdministrativeDivisions.Domain.CityDistricts.ValueObjects
{
    public class CityDistrictNameVariant
    {
        private readonly string _nameVariant;

        public CityDistrictNameVariant(string nameVariant)
        {
            if (string.IsNullOrWhiteSpace(nameVariant))
                throw new CityDistrictNameVariantNullException();

            _nameVariant = nameVariant;
        }

        public static implicit operator string(CityDistrictNameVariant nameVariant)
        {
            return nameVariant._nameVariant;
        }
    }
}