using System;
using System.Linq;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Enumerations;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Enums;
using Riva.BuildingBlocks.Domain;

namespace Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Extensions
{
    public static class NumberOfRoomsExtension
    {
        public static NumberOfRooms ConvertToEnum(this NumberOfRoomsEnumeration numberOfRooms)
        {
            switch (numberOfRooms)
            {
                case { } numberOfRoomsEnumeration when Equals(numberOfRoomsEnumeration, NumberOfRoomsEnumeration.One):
                    return NumberOfRooms.One;
                case { } numberOfRoomsEnumeration when Equals(numberOfRoomsEnumeration, NumberOfRoomsEnumeration.Two):
                    return NumberOfRooms.Two;
                case { } numberOfRoomsEnumeration when Equals(numberOfRoomsEnumeration, NumberOfRoomsEnumeration.Three):
                    return NumberOfRooms.Three;
                case { } numberOfRoomsEnumeration when Equals(numberOfRoomsEnumeration, NumberOfRoomsEnumeration.Four):
                    return NumberOfRooms.Four;
                case { } numberOfRoomsEnumeration when Equals(numberOfRoomsEnumeration, NumberOfRoomsEnumeration.FiveAndMore):
                    return NumberOfRooms.FiveAndMore;
                case { } numberOfRoomsEnumeration when Equals(numberOfRoomsEnumeration, NumberOfRoomsEnumeration.NotSpecified):
                    return NumberOfRooms.NotSpecified;
                default:
                    throw new ArgumentException($"{nameof(numberOfRooms.DisplayName)} is not supported by {nameof(NumberOfRooms)}.");
            }
        }

        public static NumberOfRoomsEnumeration ConvertToEnumeration(this NumberOfRooms numberOfRooms)
        {
            return EnumerationBase.GetAll<NumberOfRoomsEnumeration>()
                .SingleOrDefault(x => x.DisplayName.ToLower().Equals(numberOfRooms.ToString().ToLower()));
        }
    }
}