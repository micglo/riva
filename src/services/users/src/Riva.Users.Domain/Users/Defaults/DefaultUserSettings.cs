using Riva.Users.Domain.Users.Enumerations;

namespace Riva.Users.Domain.Users.Defaults
{
    public static class DefaultUserSettings
    {
        public const bool ServiceActive = true;
        public const int AnnouncementPreferenceLimit = 2;
        public static AnnouncementSendingFrequencyEnumeration AnnouncementSendingFrequency = AnnouncementSendingFrequencyEnumeration.EveryFourHours;
    }
}