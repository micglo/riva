using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Domain;

namespace Riva.Announcements.Core.Enumerations
{
    public class CityErrorCodeEnumeration : EnumerationBase, IErrorCode
    {
        public static CityErrorCodeEnumeration NotFound => new CityErrorCodeEnumeration(1, nameof(NotFound));

        private CityErrorCodeEnumeration(int value, string displayName) : base(value, displayName)
        {
        }
    }
}