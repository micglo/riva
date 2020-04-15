using System;
using System.Collections.Generic;
using System.Linq;
using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.Users.Core.Commands
{
    public class CreateFlatForRentAnnouncementPreferenceCommand : ICommand
    {
        public Guid FlatForRentAnnouncementPreferenceId { get; }
        public Guid UserId { get; }
        public Guid CityId { get; }
        public decimal? PriceMin { get; }
        public decimal? PriceMax { get; }
        public int? RoomNumbersMin { get; }
        public int? RoomNumbersMax { get; }
        public IReadOnlyCollection<Guid> CityDistricts { get; }
        public Guid CorrelationId { get; }

        public CreateFlatForRentAnnouncementPreferenceCommand(Guid userId, Guid cityId, decimal? priceMin, decimal? priceMax, 
            int? roomNumbersMin, int? roomNumbersMax, IEnumerable<Guid> cityDistricts)
        {
            FlatForRentAnnouncementPreferenceId = Guid.NewGuid();
            UserId = userId;
            CityId = cityId;
            PriceMin = priceMin;
            PriceMax = priceMax;
            RoomNumbersMin = roomNumbersMin;
            RoomNumbersMax = roomNumbersMax;
            CityDistricts = cityDistricts.ToList().AsReadOnly();
            CorrelationId = Guid.NewGuid();
        }
    }
}