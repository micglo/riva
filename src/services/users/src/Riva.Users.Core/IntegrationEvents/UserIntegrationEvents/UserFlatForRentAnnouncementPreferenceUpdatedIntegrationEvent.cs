using System;
using System.Collections.Generic;
using System.Linq;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.Users.Core.IntegrationEvents.UserIntegrationEvents
{
    public class UserFlatForRentAnnouncementPreferenceUpdatedIntegrationEvent : IntegrationEventBase
    {
        public Guid UserId { get; }
        public Guid FlatForRentAnnouncementPreferenceId { get; }
        public Guid CityId { get; }
        public decimal? PriceMin { get; }
        public decimal? PriceMax { get; }
        public int? RoomNumbersMin { get; }
        public int? RoomNumbersMax { get; }
        public IReadOnlyCollection<Guid> CityDistricts { get; }

        public UserFlatForRentAnnouncementPreferenceUpdatedIntegrationEvent(Guid correlationId, Guid userId, 
            Guid flatForRentAnnouncementPreferenceId, Guid cityId, decimal? priceMin, decimal? priceMax, 
            int? roomNumbersMin, int? roomNumbersMax, IEnumerable<Guid> cityDistricts) : base(correlationId)
        {
            UserId = userId;
            FlatForRentAnnouncementPreferenceId = flatForRentAnnouncementPreferenceId;
            CityId = cityId;
            PriceMin = priceMin;
            PriceMax = priceMax;
            RoomNumbersMin = roomNumbersMin;
            RoomNumbersMax = roomNumbersMax;
            CityDistricts = cityDistricts.ToList().AsReadOnly();
        }
    }
}