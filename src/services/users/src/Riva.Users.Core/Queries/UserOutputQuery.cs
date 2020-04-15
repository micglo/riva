using System;
using System.Collections.Generic;
using System.Linq;
using Riva.BuildingBlocks.Core.Queries;
using Riva.Users.Domain.Users.Enumerations;

namespace Riva.Users.Core.Queries
{
    public class UserOutputQuery : OutputQueryBase
    {
        public string Email { get; }
        public string Picture { get; }
        public bool ServiceActive { get; }
        public int AnnouncementPreferenceLimit { get; }
        public AnnouncementSendingFrequencyEnumeration AnnouncementSendingFrequency { get; }
        public IReadOnlyCollection<RoomForRentAnnouncementPreferenceOutputQuery> RoomForRentAnnouncementPreferences { get; }
        public IReadOnlyCollection<FlatForRentAnnouncementPreferenceOutputQuery> FlatForRentAnnouncementPreferences { get; }

        public UserOutputQuery(Guid id, string email, string picture, bool serviceActive, 
            int announcementPreferenceLimit, AnnouncementSendingFrequencyEnumeration announcementSendingFrequency, 
            IEnumerable<RoomForRentAnnouncementPreferenceOutputQuery> roomForRentAnnouncementPreferences,
            IEnumerable<FlatForRentAnnouncementPreferenceOutputQuery> flatForRentAnnouncementPreferences)
            : base(id)
        {
            Email = email;
            Picture = picture;
            ServiceActive = serviceActive;
            AnnouncementPreferenceLimit = announcementPreferenceLimit;
            AnnouncementSendingFrequency = announcementSendingFrequency;
            RoomForRentAnnouncementPreferences = roomForRentAnnouncementPreferences.ToList().AsReadOnly();
            FlatForRentAnnouncementPreferences = flatForRentAnnouncementPreferences.ToList().AsReadOnly();
        }
    }
}