using System;
using System.Collections.Generic;
using System.Linq;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.Users.Core.Enums;

namespace Riva.Users.Core.IntegrationEvents.UserIntegrationEvents
{
    public class UserRoomForRentAnnouncementPreferenceUpdatedIntegrationEvent : IntegrationEventBase
    {
        public Guid UserId { get; }
        public Guid RoomForRentAnnouncementPreferenceId { get; }
        public Guid CityId { get; }
        public decimal? PriceMin { get; }
        public decimal? PriceMax { get; }
        public RoomType? RoomType { get; }
        public IReadOnlyCollection<Guid> CityDistricts { get; }

        public UserRoomForRentAnnouncementPreferenceUpdatedIntegrationEvent(Guid correlationId, Guid userId, 
            Guid roomForRentAnnouncementPreferenceId, Guid cityId, decimal? priceMin, decimal? priceMax, 
            RoomType? roomType, IEnumerable<Guid> cityDistricts) : base(correlationId)
        {
            UserId = userId;
            RoomForRentAnnouncementPreferenceId = roomForRentAnnouncementPreferenceId;
            CityId = cityId;
            PriceMin = priceMin;
            PriceMax = priceMax;
            RoomType = roomType;
            CityDistricts = cityDistricts.ToList().AsReadOnly();
        }
    }
}