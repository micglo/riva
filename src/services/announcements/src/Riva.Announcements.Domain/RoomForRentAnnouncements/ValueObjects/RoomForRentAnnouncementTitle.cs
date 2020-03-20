using Riva.Announcements.Domain.RoomForRentAnnouncements.Exceptions;

namespace Riva.Announcements.Domain.RoomForRentAnnouncements.ValueObjects
{
    public class RoomForRentAnnouncementTitle
    {
        private const int MaxLength = 256;
        private readonly string _title;

        public RoomForRentAnnouncementTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new RoomForRentAnnouncementTitleNullException();
            if (title.Length > MaxLength)
                throw new RoomForRentAnnouncementTitleMaxLengthException(MaxLength);

            _title = title;
        }

        public static implicit operator string(RoomForRentAnnouncementTitle title)
        {
            return title._title;
        }
    }
}