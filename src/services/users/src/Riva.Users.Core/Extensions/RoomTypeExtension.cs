using System;
using System.Linq;
using Riva.BuildingBlocks.Domain;
using Riva.Users.Domain.Users.Enumerations;
using Riva.Users.Core.Enums;

namespace Riva.Users.Core.Extensions
{
    public static class RoomTypeExtension
    {
        public static RoomType? ConvertToEnum(this RoomTypeEnumeration roomType)
        {
            if (roomType is null)
                return null;

            switch (roomType)
            {
                case { } roomTypeEnumeration when Equals(roomTypeEnumeration, RoomTypeEnumeration.Single):
                    return RoomType.Single;
                case { } roomTypeEnumeration when Equals(roomTypeEnumeration, RoomTypeEnumeration.Double):
                    return RoomType.Double;
                case { } roomTypeEnumeration when Equals(roomTypeEnumeration, RoomTypeEnumeration.Triple):
                    return RoomType.Triple;
                case { } roomTypeEnumeration when Equals(roomTypeEnumeration, RoomTypeEnumeration.Quadruple):
                    return RoomType.Quadruple;
                case { } roomTypeEnumeration when Equals(roomTypeEnumeration, RoomTypeEnumeration.MultiPerson):
                    return RoomType.MultiPerson;
                default:
                    throw new ArgumentException($"{nameof(roomType.DisplayName)} is not supported by {nameof(RoomType)}.");
            }
        }

        public static RoomTypeEnumeration ConvertToEnumeration(this RoomType? roomType)
        {
            return !roomType.HasValue ? 
                null : 
                EnumerationBase.GetAll<RoomTypeEnumeration>().SingleOrDefault(x => x.DisplayName.ToLower().Equals(roomType.ToString().ToLower()));
        }
    }
}