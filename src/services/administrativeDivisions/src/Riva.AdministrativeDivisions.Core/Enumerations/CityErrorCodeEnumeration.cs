using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Domain;

namespace Riva.AdministrativeDivisions.Core.Enumerations
{
    public class CityErrorCodeEnumeration : EnumerationBase, IErrorCode
    {
        public static CityErrorCodeEnumeration NotFound => new CityErrorCodeEnumeration(1, nameof(NotFound));
        public static CityErrorCodeEnumeration NameAlreadyInUse => new CityErrorCodeEnumeration(2, nameof(NameAlreadyInUse));
        public static CityErrorCodeEnumeration PolishNameAlreadyInUse => new CityErrorCodeEnumeration(3, nameof(PolishNameAlreadyInUse));

        private CityErrorCodeEnumeration(int value, string displayName) : base(value, displayName)
        {
        }
    }
}