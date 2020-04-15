using Riva.BuildingBlocks.Core.Models;
using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Core.Enumerations
{
    public class FlatForRentAnnouncementPreferenceErrorCode : EnumerationBase, IErrorCode
    {
        public static FlatForRentAnnouncementPreferenceErrorCode NotFound => new FlatForRentAnnouncementPreferenceErrorCode(1, nameof(NotFound));
        public static FlatForRentAnnouncementPreferenceErrorCode ExpansibleCityDistricts => new FlatForRentAnnouncementPreferenceErrorCode(3, nameof(ExpansibleCityDistricts));
        public static FlatForRentAnnouncementPreferenceErrorCode ChangeableRoomNumbers => new FlatForRentAnnouncementPreferenceErrorCode(4, nameof(ChangeableRoomNumbers));
        public static FlatForRentAnnouncementPreferenceErrorCode ChangeablePrices => new FlatForRentAnnouncementPreferenceErrorCode(5, nameof(ChangeablePrices));

        private FlatForRentAnnouncementPreferenceErrorCode(int value, string displayName) : base(value, displayName)
        {
        }
    }
}