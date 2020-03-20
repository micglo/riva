using System;
using System.Collections.Generic;
using System.Linq;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Enumerations;
using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.Announcements.Core.Commands
{
    public class UpdateFlatForRentAnnouncementCommand : ICommand
    {
        public Guid FlatForRentAnnouncementId { get; }
        public string Title { get; }
        public string SourceUrl { get; }
        public Guid CityId { get; }
        public string Description { get; }
        public decimal? Price { get; }
        public NumberOfRoomsEnumeration NumberOfRooms { get; }
        public IReadOnlyCollection<Guid> CityDistricts { get; }

        public UpdateFlatForRentAnnouncementCommand(Guid flatForRentAnnouncementId, string title, string sourceUrl, Guid cityId,
            string description, decimal? price, NumberOfRoomsEnumeration numberOfRooms, IEnumerable<Guid> cityDistricts)
        {
            FlatForRentAnnouncementId = flatForRentAnnouncementId;
            Title = title;
            SourceUrl = sourceUrl;
            CityId = cityId;
            Description = description;
            Price = price;
            NumberOfRooms = numberOfRooms;
            CityDistricts = cityDistricts.ToList().AsReadOnly();
        }
    }
}