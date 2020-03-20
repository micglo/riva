using Riva.BuildingBlocks.Domain;

namespace Riva.Announcements.Domain.FlatForRentAnnouncements.Enumerations
{
    public class NumberOfRoomsEnumeration : EnumerationBase
    {
        public static NumberOfRoomsEnumeration One => new NumberOfRoomsEnumeration(1, nameof(One));
        public static NumberOfRoomsEnumeration Two => new NumberOfRoomsEnumeration(2, nameof(Two));
        public static NumberOfRoomsEnumeration Three => new NumberOfRoomsEnumeration(3, nameof(Three));
        public static NumberOfRoomsEnumeration Four => new NumberOfRoomsEnumeration(4, nameof(Four));
        public static NumberOfRoomsEnumeration FiveAndMore => new NumberOfRoomsEnumeration(5, nameof(FiveAndMore));
        public static NumberOfRoomsEnumeration NotSpecified => new NumberOfRoomsEnumeration(6, nameof(NotSpecified));

        private NumberOfRoomsEnumeration(int value, string displayName) : base(value, displayName)
        {
        }
    }
}