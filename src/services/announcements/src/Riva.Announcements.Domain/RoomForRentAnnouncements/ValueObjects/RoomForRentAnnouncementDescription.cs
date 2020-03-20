using Riva.Announcements.Domain.RoomForRentAnnouncements.Exceptions;

namespace Riva.Announcements.Domain.RoomForRentAnnouncements.ValueObjects
{
    public class RoomForRentAnnouncementDescription
    {
        private readonly string _description;

        public RoomForRentAnnouncementDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new RoomForRentAnnouncementDescriptionNullException();

            _description = description;
        }

        public static implicit operator string(RoomForRentAnnouncementDescription description)
        {
            return description._description;
        }
    }
}