using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Domain;

namespace Riva.Announcements.Core.Enumerations
{
    public class FlatForRentAnnouncementErrorCodeEnumeration : EnumerationBase, IErrorCode
    {
        public static FlatForRentAnnouncementErrorCodeEnumeration NotFound => new FlatForRentAnnouncementErrorCodeEnumeration(1, nameof(NotFound));

        private FlatForRentAnnouncementErrorCodeEnumeration(int value, string displayName) : base(value, displayName)
        {
        }
    }
}