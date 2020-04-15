using System;
using System.Collections.Generic;
using Riva.Users.Domain.Users.Aggregates;
using Riva.Users.Domain.Users.Entities;
using Riva.Users.Domain.Users.Enumerations;

namespace Riva.Users.Domain.Users.Builders
{
    public interface IUserIdSetter
    {
        IUserEmailSetter SetId(Guid id);
    }

    public interface IUserEmailSetter
    {
        IUserServiceActiveSetter SetEmail(string email);
    }

    public interface IUserServiceActiveSetter
    {
        IUserAnnouncementPreferenceLimitSetter SetServiceActive(bool serviceActive);
    }

    public interface IUserAnnouncementPreferenceLimitSetter
    {
        IUserAnnouncementSendingFrequencySetter SetAnnouncementPreferenceLimit(int announcementPreferenceLimit);
    }

    public interface IUserAnnouncementSendingFrequencySetter
    {
        IUserBuilder SetAnnouncementSendingFrequency(AnnouncementSendingFrequencyEnumeration announcementSendingFrequency);
    }

    public interface IUserBuilder
    {
        IUserBuilder SetPicture(string picture);
        IUserBuilder SetRoomForRentAnnouncementPreferences(IEnumerable<RoomForRentAnnouncementPreference> roomForRentAnnouncementPreferences);
        IUserBuilder SetFlatForRentAnnouncementPreferences(IEnumerable<FlatForRentAnnouncementPreference> flatForRentAnnouncementPreferences);
        User Build();
    }
}