using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Domain;

namespace Riva.Announcements.Core.Enumerations
{
    public class CityDistrictErrorCodeEnumeration : EnumerationBase, IErrorCode
    {
        public static CityDistrictErrorCodeEnumeration NotFound => new CityDistrictErrorCodeEnumeration(1, nameof(NotFound));

        private CityDistrictErrorCodeEnumeration(int value, string displayName) : base(value, displayName)
        {
        }
    }
}