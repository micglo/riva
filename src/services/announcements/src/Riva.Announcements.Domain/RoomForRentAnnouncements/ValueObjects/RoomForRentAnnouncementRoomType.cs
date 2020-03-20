using Riva.Announcements.Domain.RoomForRentAnnouncements.Enumerations;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Exceptions;

namespace Riva.Announcements.Domain.RoomForRentAnnouncements.ValueObjects
{
    public class RoomForRentAnnouncementRoomType
    {
        private readonly RoomTypeEnumeration _roomType;

        public RoomForRentAnnouncementRoomType(RoomTypeEnumeration roomType)
        {
            _roomType = roomType ?? throw new RoomForRentAnnouncementRoomTypeNullException();
        }

        public static implicit operator RoomTypeEnumeration(RoomForRentAnnouncementRoomType roomType)
        {
            return roomType._roomType;
        }
    }
}