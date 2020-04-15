using System;
using System.Linq;
using Riva.BuildingBlocks.Domain;
using Riva.Users.Domain.Users.Enumerations;
using Riva.Users.Core.Enums;

namespace Riva.Users.Core.Extensions
{
    public static class AnnouncementSendingFrequencyExtension
    {
        public static AnnouncementSendingFrequency ConvertToEnum(this AnnouncementSendingFrequencyEnumeration announcementSendingFrequency)
        {
            switch (announcementSendingFrequency)
            {
                case { } announcementSendingFrequencyEnumeration when Equals(announcementSendingFrequencyEnumeration, AnnouncementSendingFrequencyEnumeration.EveryHour):
                    return AnnouncementSendingFrequency.EveryHour;
                case { } announcementSendingFrequencyEnumeration when Equals(announcementSendingFrequencyEnumeration, AnnouncementSendingFrequencyEnumeration.EveryTwoHours):
                    return AnnouncementSendingFrequency.EveryTwoHours;
                case { } announcementSendingFrequencyEnumeration when Equals(announcementSendingFrequencyEnumeration, AnnouncementSendingFrequencyEnumeration.EveryThreeHours):
                    return AnnouncementSendingFrequency.EveryThreeHours;
                case { } announcementSendingFrequencyEnumeration when Equals(announcementSendingFrequencyEnumeration, AnnouncementSendingFrequencyEnumeration.EveryFourHours):
                    return AnnouncementSendingFrequency.EveryFourHours;
                case { } announcementSendingFrequencyEnumeration when Equals(announcementSendingFrequencyEnumeration, AnnouncementSendingFrequencyEnumeration.EveryFiveHours):
                    return AnnouncementSendingFrequency.EveryFiveHours;
                case { } announcementSendingFrequencyEnumeration when Equals(announcementSendingFrequencyEnumeration, AnnouncementSendingFrequencyEnumeration.EverySixHours):
                    return AnnouncementSendingFrequency.EverySixHours;
                case { } announcementSendingFrequencyEnumeration when Equals(announcementSendingFrequencyEnumeration, AnnouncementSendingFrequencyEnumeration.EveryTwelveHours):
                    return AnnouncementSendingFrequency.EveryTwelveHours;
                default:
                    throw new ArgumentException($"{nameof(announcementSendingFrequency.DisplayName)} is not supported by {nameof(AnnouncementSendingFrequency)}.");
            }
        }

        public static AnnouncementSendingFrequencyEnumeration ConvertToEnumeration(this AnnouncementSendingFrequency announcementSendingFrequency)
        {
            return EnumerationBase.GetAll<AnnouncementSendingFrequencyEnumeration>()
                .SingleOrDefault(x => x.DisplayName.ToLower().Equals(announcementSendingFrequency.ToString().ToLower()));
        }
    }
}