using System;
using System.Collections.Generic;
using System.Linq;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Enumerations;
using Riva.BuildingBlocks.Core.Communications.Commands;

namespace Riva.Announcements.Core.Commands
{
    public class UpdateRoomForRentAnnouncementCommand : ICommand
    {
        public Guid RoomForRentAnnouncementId { get; }
        public string Title { get; }
        public string SourceUrl { get; }
        public Guid CityId { get; }
        public string Description { get; }
        public decimal? Price { get; }
        public IReadOnlyCollection<RoomTypeEnumeration> RoomTypes { get; }
        public IReadOnlyCollection<Guid> CityDistricts { get; }

        public UpdateRoomForRentAnnouncementCommand(Guid roomForRentAnnouncementId, string title, string sourceUrl, Guid cityId,
            string description, decimal? price, IEnumerable<RoomTypeEnumeration> roomTypes, IEnumerable<Guid> cityDistricts)
        {
            RoomForRentAnnouncementId = roomForRentAnnouncementId;
            Title = title;
            SourceUrl = sourceUrl;
            CityId = cityId;
            Description = description;
            Price = price;
            RoomTypes = roomTypes.ToList().AsReadOnly();
            CityDistricts = cityDistricts.ToList().AsReadOnly();
        }
    }
}