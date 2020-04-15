using System;
using System.Collections.Generic;
using System.Linq;
using Riva.BuildingBlocks.WebApi.Models.Responses;
using Riva.Users.Core.Enums;

namespace Riva.Users.Web.Api.Models.Responses
{
    public class UserResponse : ResponseBase
    {
        public string Email { get; }
        public string Picture { get; }
        public bool ServiceActive { get; }
        public int AnnouncementPreferenceLimit { get; }
        public AnnouncementSendingFrequency AnnouncementSendingFrequency { get; }
        public IReadOnlyCollection<RoomForRentAnnouncementPreferenceResponse> RoomForRentAnnouncementPreferences { get; }
        public IReadOnlyCollection<FlatForRentAnnouncementPreferenceResponse> FlatForRentAnnouncementPreferences { get; }

        public UserResponse(Guid id, string email, string picture, bool serviceActive, 
            int announcementPreferenceLimit, AnnouncementSendingFrequency announcementSendingFrequency, 
            IEnumerable<RoomForRentAnnouncementPreferenceResponse> roomForRentAnnouncementPreferences, 
            IEnumerable<FlatForRentAnnouncementPreferenceResponse> flatForRentAnnouncementPreferences)
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