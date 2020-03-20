using Riva.Announcements.Domain.RoomForRentAnnouncements.Exceptions;

namespace Riva.Announcements.Domain.RoomForRentAnnouncements.ValueObjects
{
    public class RoomForRentAnnouncementSourceUrl
    {
        private readonly string _sourceUrl;

        public RoomForRentAnnouncementSourceUrl(string sourceUrl)
        {
            if (string.IsNullOrWhiteSpace(sourceUrl))
                throw new RoomForRentAnnouncementSourceUrlNullException();

            _sourceUrl = sourceUrl;
        }

        public static implicit operator string(RoomForRentAnnouncementSourceUrl sourceUrl)
        {
            return sourceUrl._sourceUrl;
        }
    }
}