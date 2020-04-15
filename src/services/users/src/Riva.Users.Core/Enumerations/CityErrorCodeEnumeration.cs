using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Core.Enumerations
{
    public class CityErrorCodeEnumeration : EnumerationBase, IErrorCode
    {
        public static CityErrorCodeEnumeration NotFound => new CityErrorCodeEnumeration(1, nameof(NotFound));
        public static CityErrorCodeEnumeration IncorrectCityDistricts => new CityErrorCodeEnumeration(2, nameof(IncorrectCityDistricts));

        private CityErrorCodeEnumeration(int value, string displayName) : base(value, displayName)
        {
        }
    }
}