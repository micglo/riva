using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Enumerations
{
    public class RoomTypeEnumeration : EnumerationBase
    {
        public static RoomTypeEnumeration Single => new RoomTypeEnumeration(1, nameof(Single));
        public static RoomTypeEnumeration Double => new RoomTypeEnumeration(2, nameof(Double));
        public static RoomTypeEnumeration Triple => new RoomTypeEnumeration(3, nameof(Triple));
        public static RoomTypeEnumeration Quadruple => new RoomTypeEnumeration(4, nameof(Quadruple));
        public static RoomTypeEnumeration MultiPerson => new RoomTypeEnumeration(5, nameof(MultiPerson));

        private RoomTypeEnumeration(int value, string displayName) : base(value, displayName)
        {
        }
    }
}