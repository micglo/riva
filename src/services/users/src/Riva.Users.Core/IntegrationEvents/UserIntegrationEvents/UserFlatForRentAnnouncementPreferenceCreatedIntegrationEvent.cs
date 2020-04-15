using System;
using System.Collections.Generic;
using System.Linq;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.Users.Core.Enums;

namespace Riva.Users.Core.IntegrationEvents.UserIntegrationEvents
{
    public class UserFlatForRentAnnouncementPreferenceCreatedIntegrationEvent : IntegrationEventBase
    {
        public Guid UserId { get; }
        public Guid FlatForRentAnnouncementPreferenceId { get; }
        public string UserEmail { get; }
        public Guid CityId { get; }
        public bool ServiceActive { get; }
        public AnnouncementSendingFrequency AnnouncementSendingFrequency { get; }
        public decimal? PriceMin { get; }
        public decimal? PriceMax { get; }
        public int? RoomNumbersMin { get; }
        public int? RoomNumbersMax { get; }
        public IReadOnlyCollection<Guid> CityDistricts { get; }

        public UserFlatForRentAnnouncementPreferenceCreatedIntegrationEvent(Guid correlationId, Guid userId, 
            Guid flatForRentAnnouncementPreferenceId, string userEmail, Guid cityId, bool serviceActive, 
            AnnouncementSendingFrequency announcementSendingFrequency, decimal? priceMin, decimal? priceMax, 
            int? roomNumbersMin, int? roomNumbersMax, IEnumerable<Guid> cityDistricts) : base(correlationId)
        {
            UserId = userId;
            FlatForRentAnnouncementPreferenceId = flatForRentAnnouncementPreferenceId;
            UserEmail = userEmail;
            CityId = cityId;
            ServiceActive = serviceActive;
            AnnouncementSendingFrequency = announcementSendingFrequency;
            PriceMin = priceMin;
            PriceMax = priceMax;
            RoomNumbersMin = roomNumbersMin;
            RoomNumbersMax = roomNumbersMax;
            CityDistricts = cityDistricts.ToList().AsReadOnly();
        }
    }
}