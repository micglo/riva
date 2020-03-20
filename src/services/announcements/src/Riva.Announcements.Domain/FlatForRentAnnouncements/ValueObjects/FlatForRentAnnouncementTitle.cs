using Riva.Announcements.Domain.FlatForRentAnnouncements.Exceptions;

namespace Riva.Announcements.Domain.FlatForRentAnnouncements.ValueObjects
{
    public class FlatForRentAnnouncementTitle
    {
        private const int MaxLength = 256;
        private readonly string _title;

        public FlatForRentAnnouncementTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new FlatForRentAnnouncementTitleNullException();
            if (title.Length > MaxLength)
                throw new FlatForRentAnnouncementTitleMaxLengthException(MaxLength);

            _title = title;
        }

        public static implicit operator string(FlatForRentAnnouncementTitle title)
        {
            return title._title;
        }
    }
}