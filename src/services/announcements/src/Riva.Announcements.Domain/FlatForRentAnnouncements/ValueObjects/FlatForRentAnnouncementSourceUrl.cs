using Riva.Announcements.Domain.FlatForRentAnnouncements.Exceptions;

namespace Riva.Announcements.Domain.FlatForRentAnnouncements.ValueObjects
{
    public class FlatForRentAnnouncementSourceUrl
    {
        private readonly string _sourceUrl;

        public FlatForRentAnnouncementSourceUrl(string sourceUrl)
        {
            if (string.IsNullOrWhiteSpace(sourceUrl))
                throw new FlatForRentAnnouncementSourceUrlNullException();

            _sourceUrl = sourceUrl;
        }

        public static implicit operator string(FlatForRentAnnouncementSourceUrl sourceUrl)
        {
            return sourceUrl._sourceUrl;
        }
    }
}