using System;
using System.Collections.Generic;
using System.Linq;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.Users.Core.Enums;

namespace Riva.Users.Core.IntegrationEvents.UserIntegrationEvents
{
    public class UserRoomForRentAnnouncementPreferenceCreatedIntegrationEvent : IntegrationEventBase
    {
        public Guid UserId { get; }
        public Guid RoomForRentAnnouncementPreferenceId { get; }
        public string UserEmail { get; }
        public Guid CityId { get; }
        public bool ServiceActive { get; }
        public AnnouncementSendingFrequency AnnouncementSendingFrequency { get; }
        public decimal? PriceMin { get; }
        public decimal? PriceMax { get; }
        public RoomType? RoomType { get; }
        public IReadOnlyCollection<Guid> CityDistricts { get; }

        public UserRoomForRentAnnouncementPreferenceCreatedIntegrationEvent(Guid correlationId, Guid userId, 
            Guid roomForRentAnnouncementPreferenceId, string userEmail, Guid cityId, bool serviceActive, 
            AnnouncementSendingFrequency announcementSendingFrequency, decimal? priceMin, decimal? priceMax, 
            RoomType? roomType, IEnumerable<Guid> cityDistricts) : base(correlationId)
        {
            UserId = userId;
            RoomForRentAnnouncementPreferenceId = roomForRentAnnouncementPreferenceId;
            UserEmail = userEmail;
            CityId = cityId;
            ServiceActive = serviceActive;
            AnnouncementSendingFrequency = announcementSendingFrequency;
            PriceMin = priceMin;
            PriceMax = priceMax;
            RoomType = roomType;
            CityDistricts = cityDistricts.ToList().AsReadOnly();
        }
    }
}