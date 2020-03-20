using System.Collections.Generic;
using System.Linq;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Enumerations;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Exceptions;

namespace Riva.Announcements.Domain.RoomForRentAnnouncements.ValueObjects
{
    public class RoomForRentAnnouncementRoomTypes
    {
        private readonly List<RoomTypeEnumeration> _roomTypes;

        public RoomForRentAnnouncementRoomTypes(IEnumerable<RoomTypeEnumeration> roomTypes)
        {
            if (roomTypes is null)
                throw new RoomForRentAnnouncementRoomTypesNullException();

            var roomTypeList = roomTypes.ToList();

            if (roomTypeList.Any(x => x is null))
                throw new RoomForRentAnnouncementRoomTypesNullException();

            var anyDuplicate = roomTypeList.GroupBy(x => x).Any(g => g.Count() > 1);
            if (anyDuplicate)
                throw new RoomForRentAnnouncementRoomTypesDuplicateValuesException();

            _roomTypes = new List<RoomTypeEnumeration>(roomTypeList);
        }

        public static implicit operator List<RoomTypeEnumeration>(RoomForRentAnnouncementRoomTypes roomTypes)
        {
            return roomTypes._roomTypes;
        }
    }
}