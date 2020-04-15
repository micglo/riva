using Riva.BuildingBlocks.Domain;

namespace Riva.Users.Domain.Users.Enumerations
{
    public class AnnouncementSendingFrequencyEnumeration : EnumerationBase
    {
        public static AnnouncementSendingFrequencyEnumeration EveryHour => new AnnouncementSendingFrequencyEnumeration(1, nameof(EveryHour));
        public static AnnouncementSendingFrequencyEnumeration EveryTwoHours => new AnnouncementSendingFrequencyEnumeration(2, nameof(EveryTwoHours));
        public static AnnouncementSendingFrequencyEnumeration EveryThreeHours => new AnnouncementSendingFrequencyEnumeration(3, nameof(EveryThreeHours));
        public static AnnouncementSendingFrequencyEnumeration EveryFourHours => new AnnouncementSendingFrequencyEnumeration(4, nameof(EveryFourHours));
        public static AnnouncementSendingFrequencyEnumeration EveryFiveHours => new AnnouncementSendingFrequencyEnumeration(5, nameof(EveryFiveHours));
        public static AnnouncementSendingFrequencyEnumeration EverySixHours => new AnnouncementSendingFrequencyEnumeration(6, nameof(EverySixHours));
        public static AnnouncementSendingFrequencyEnumeration EveryTwelveHours => new AnnouncementSendingFrequencyEnumeration(7, nameof(EveryTwelveHours));

        public AnnouncementSendingFrequencyEnumeration(int value, string displayName) : base(value, displayName)
        {
        }
    }
}