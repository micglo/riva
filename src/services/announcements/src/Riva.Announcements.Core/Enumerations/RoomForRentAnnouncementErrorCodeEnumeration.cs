using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Domain;

namespace Riva.Announcements.Core.Enumerations
{
    public class RoomForRentAnnouncementErrorCodeEnumeration : EnumerationBase, IErrorCode
    {
        public static RoomForRentAnnouncementErrorCodeEnumeration NotFound => new RoomForRentAnnouncementErrorCodeEnumeration(1, nameof(NotFound));

        private RoomForRentAnnouncementErrorCodeEnumeration(int value, string displayName) : base(value, displayName)
        {
        }
    }
}