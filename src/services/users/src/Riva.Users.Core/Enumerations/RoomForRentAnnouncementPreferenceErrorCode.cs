using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Core.Enumerations
{
    public class RoomForRentAnnouncementPreferenceErrorCode : EnumerationBase, IErrorCode
    {
        public static RoomForRentAnnouncementPreferenceErrorCode NotFound => new RoomForRentAnnouncementPreferenceErrorCode(1, nameof(NotFound));
        public static RoomForRentAnnouncementPreferenceErrorCode ExpansibleCityDistricts => new RoomForRentAnnouncementPreferenceErrorCode(2, nameof(ExpansibleCityDistricts));
        public static RoomForRentAnnouncementPreferenceErrorCode ChangeablePrices => new RoomForRentAnnouncementPreferenceErrorCode(3, nameof(ChangeablePrices));

        private RoomForRentAnnouncementPreferenceErrorCode(int value, string displayName) : base(value, displayName)
        {
        }
    }
}