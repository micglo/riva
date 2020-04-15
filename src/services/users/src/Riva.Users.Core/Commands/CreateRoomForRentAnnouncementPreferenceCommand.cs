using System;
using System.Collections.Generic;
using System.Linq;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.Users.Domain.Users.Enumerations;

namespace Riva.Users.Core.Commands
{
    public class CreateRoomForRentAnnouncementPreferenceCommand : ICommand
    {
        public Guid RoomForRentAnnouncementPreferenceId { get; }
        public Guid UserId { get; }
        public Guid CityId { get; }
        public decimal? PriceMin { get; }
        public decimal? PriceMax { get; }
        public RoomTypeEnumeration RoomType { get; }
        public IReadOnlyCollection<Guid> CityDistricts { get; }
        public Guid CorrelationId { get; }

        public CreateRoomForRentAnnouncementPreferenceCommand(Guid userId, Guid cityId, decimal? priceMin, decimal? priceMax, RoomTypeEnumeration roomType, IEnumerable<Guid> cityDistricts)
        {
            RoomForRentAnnouncementPreferenceId = Guid.NewGuid();
            UserId = userId;
            CityId = cityId;
            PriceMin = priceMin;
            PriceMax = priceMax;
            RoomType = roomType;
            CityDistricts = cityDistricts.ToList().AsReadOnly();
            CorrelationId = Guid.NewGuid();
        }
    }
}