using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Domain;

namespace Riva.AdministrativeDivisions.Core.Enumerations
{
    public class CityDistrictErrorCodeEnumeration : EnumerationBase, IErrorCode
    {
        public static CityDistrictErrorCodeEnumeration NotFound => new CityDistrictErrorCodeEnumeration(1, nameof(NotFound));
        public static CityDistrictErrorCodeEnumeration ParentNotFound => new CityDistrictErrorCodeEnumeration(2, nameof(ParentNotFound));
        public static CityDistrictErrorCodeEnumeration NameAlreadyInUse => new CityDistrictErrorCodeEnumeration(3, nameof(NameAlreadyInUse));
        public static CityDistrictErrorCodeEnumeration PolishNameAlreadyInUse => new CityDistrictErrorCodeEnumeration(4, nameof(PolishNameAlreadyInUse));

        private CityDistrictErrorCodeEnumeration(int value, string displayName) : base(value, displayName)
        {
        }
    }
}