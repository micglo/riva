using Riva.Users.Domain.Users.Exceptions.AggregateExceptions;

namespace Riva.Users.Domain.Users.ValueObjects.AggregateValueObjects
{
    public class UserAnnouncementPreferenceLimit
    {
        private readonly int _announcementPreferenceLimit;
        private const int MinValue = 1;

        public UserAnnouncementPreferenceLimit(int announcementPreferenceLimit)
        {
            if (announcementPreferenceLimit < MinValue)
                throw new UserAnnouncementPreferenceLimitMinValueException(MinValue);

            _announcementPreferenceLimit = announcementPreferenceLimit;
        }

        public static implicit operator int(UserAnnouncementPreferenceLimit announcementPreferenceLimit)
        {
            return announcementPreferenceLimit._announcementPreferenceLimit;
        }
    }
}