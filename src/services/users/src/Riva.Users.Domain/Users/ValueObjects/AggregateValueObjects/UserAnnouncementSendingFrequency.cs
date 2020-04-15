using Riva.Users.Domain.Users.Enumerations;
using Riva.Users.Domain.Users.Exceptions.AggregateExceptions;

namespace Riva.Users.Domain.Users.ValueObjects.AggregateValueObjects
{
    public class UserAnnouncementSendingFrequency
    {
        private readonly AnnouncementSendingFrequencyEnumeration _announcementSendingFrequency;

        public UserAnnouncementSendingFrequency(AnnouncementSendingFrequencyEnumeration announcementSendingFrequency)
        {
            _announcementSendingFrequency = announcementSendingFrequency ?? throw new UserAnnouncementSendingFrequencyNullException();
        }

        public static implicit operator AnnouncementSendingFrequencyEnumeration(UserAnnouncementSendingFrequency announcementSendingFrequency)
        {
            return announcementSendingFrequency._announcementSendingFrequency;
        }
    }
}