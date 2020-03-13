using System.Collections.Generic;
using System.Linq;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Exceptions;

namespace Riva.AdministrativeDivisions.Domain.CityDistricts.ValueObjects
{
    public class CityDistrictNameVariants
    {
        private readonly List<string> _nameVariants;

        public CityDistrictNameVariants(IEnumerable<string> nameVariants)
        {
            if (nameVariants is null)
                throw new CityDistrictNameVariantsNullException();

            var nameVariantList = nameVariants.ToList();

            if (nameVariantList.Any(string.IsNullOrWhiteSpace))
                throw new CityDistrictNameVariantsInvalidValueException();

            var anyDuplicate = nameVariantList.GroupBy(x => x).Any(g => g.Count() > 1);
            if (anyDuplicate)
                throw new CityDistrictNameVariantsDuplicatedValuesException();

            _nameVariants = new List<string>(nameVariantList);
        }

        public static implicit operator List<string>(CityDistrictNameVariants nameVariants)
        {
            return nameVariants._nameVariants;
        }
    }
}