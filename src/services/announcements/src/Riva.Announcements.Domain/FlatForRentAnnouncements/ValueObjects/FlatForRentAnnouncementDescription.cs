using Riva.Announcements.Domain.FlatForRentAnnouncements.Exceptions;

namespace Riva.Announcements.Domain.FlatForRentAnnouncements.ValueObjects
{
    public class FlatForRentAnnouncementDescription
    {
        private readonly string _description;

        public FlatForRentAnnouncementDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new FlatForRentAnnouncementDescriptionNullException();

            _description = description;
        }

        public static implicit operator string(FlatForRentAnnouncementDescription description)
        {
            return description._description;
        }
    }
}