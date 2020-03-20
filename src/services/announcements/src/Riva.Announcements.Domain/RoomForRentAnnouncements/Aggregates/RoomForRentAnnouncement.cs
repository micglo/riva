using System;
using System.Collections.Generic;
using System.Linq;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Builders;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Enumerations;
using Riva.Announcements.Domain.RoomForRentAnnouncements.ValueObjects;
using Riva.BuildingBlocks.Domain;

namespace Riva.Announcements.Domain.RoomForRentAnnouncements.Aggregates
{
    public class RoomForRentAnnouncement : AggregateBase
    {
        private readonly List<Guid> _cityDistricts;
        private readonly List<RoomTypeEnumeration> _roomTypes;

        public string Title { get; private set; }
        public string SourceUrl { get; private set; }
        public Guid CityId { get; private set; }
        public DateTimeOffset Created { get; }
        public string Description { get; private set; }
        public decimal? Price { get; private set; }
        public IReadOnlyCollection<Guid> CityDistricts => _cityDistricts.AsReadOnly();
        public IReadOnlyCollection<RoomTypeEnumeration> RoomTypes => _roomTypes.AsReadOnly();

        private RoomForRentAnnouncement(RoomForRentAnnouncementBuilder builder) : base(builder.Id)
        {
            Title = builder.Title;
            SourceUrl = builder.SourceUrl;
            CityId = builder.CityId;
            Created = builder.Created;
            Description = builder.Description;
            Price = builder.Price;
            _cityDistricts = builder.CityDistricts;
            _roomTypes = builder.RoomTypes;
        }

        public static IRoomForRentAnnouncementIdSetter Builder()
        {
            return new RoomForRentAnnouncementBuilder();
        }

        public void ChangeTitle(string title)
        {
            Title = new RoomForRentAnnouncementTitle(title);
        }

        public void ChangeSourceUrl(string sourceUrl)
        {
            SourceUrl = new RoomForRentAnnouncementSourceUrl(sourceUrl);
        }

        public void ChangeCityId(Guid cityId)
        {
            CityId = new RoomForRentAnnouncementCityId(cityId);
        }

        public void ChangeDescription(string description)
        {
            Description = new RoomForRentAnnouncementDescription(description);
        }

        public void ChangePrice(decimal? price)
        {
            Price = price;
        }

        public void AddCityDistrict(Guid cityDistrict)
        {
            cityDistrict = new RoomForRentAnnouncementCityDistrict(cityDistrict);
            var anyDuplicates = _cityDistricts.Contains(cityDistrict);
            if(!anyDuplicates)
                _cityDistricts.Add(cityDistrict);
        }

        public void RemoveCityDistrict(Guid cityDistrict)
        {
            _cityDistricts.Remove(new RoomForRentAnnouncementCityDistrict(cityDistrict));
        }

        public void AddRoomType(RoomTypeEnumeration roomType)
        {
            roomType = new RoomForRentAnnouncementRoomType(roomType);
            var anyDuplicates = _roomTypes.Contains(roomType);
            if (!anyDuplicates)
                _roomTypes.Add(roomType);
        }

        public void RemoveRoomType(RoomTypeEnumeration roomType)
        {
            roomType = new RoomForRentAnnouncementRoomType(roomType);
            var roomTypeToRemove = _roomTypes.SingleOrDefault(x => Equals(x, roomType));
            _roomTypes.Remove(roomTypeToRemove);
        }

        private class RoomForRentAnnouncementBuilder : IRoomForRentAnnouncementIdSetter, IRoomForRentAnnouncementTitleSetter, 
            IRoomForRentAnnouncementSourceUrlSetter, IRoomForRentAnnouncementCityIdSetter, IRoomForRentAnnouncementCreatedSetter, 
            IRoomForRentAnnouncementDescriptionSetter, IRoomForRentAnnouncementBuilder
        {
            public Guid Id { get; private set; }
            public string Title { get; private set; }
            public string SourceUrl { get; private set; }
            public Guid CityId { get; private set; }
            public DateTimeOffset Created { get; private set; }
            public string Description { get; private set; }
            public decimal? Price { get; private set; }
            public List<Guid> CityDistricts { get; private set; } = new List<Guid>();
            public List<RoomTypeEnumeration> RoomTypes { get; private set; } = new List<RoomTypeEnumeration>();

            public IRoomForRentAnnouncementTitleSetter SetId(Guid id)
            {
                Id = id;
                return this;
            }

            public IRoomForRentAnnouncementSourceUrlSetter SetTitle(string title)
            {
                Title = new RoomForRentAnnouncementTitle(title);
                return this;
            }

            public IRoomForRentAnnouncementCityIdSetter SetSourceUrl(string sourceUrl)
            {
                SourceUrl = new RoomForRentAnnouncementSourceUrl(sourceUrl);
                return this;
            }

            public IRoomForRentAnnouncementCreatedSetter SetCityId(Guid cityId)
            {
                CityId = new RoomForRentAnnouncementCityId(cityId);
                return this;
            }

            public IRoomForRentAnnouncementDescriptionSetter SetCreated(DateTimeOffset created)
            {
                Created = created;
                return this;
            }

            public IRoomForRentAnnouncementBuilder SetDescription(string description)
            {
                Description = new RoomForRentAnnouncementDescription(description);
                return this;
            }

            public IRoomForRentAnnouncementBuilder SetPrice(decimal? price)
            {
                Price = price;
                return this;
            }

            public IRoomForRentAnnouncementBuilder SetRoomTypes(IEnumerable<RoomTypeEnumeration> roomTypes)
            {
                RoomTypes = new RoomForRentAnnouncementRoomTypes(roomTypes);
                return this;
            }

            public IRoomForRentAnnouncementBuilder SetCityDistricts(IEnumerable<Guid> cityDistricts)
            {
                CityDistricts = new RoomForRentAnnouncementCityDistricts(cityDistricts);
                return this;
            }

            public RoomForRentAnnouncement Build()
            {
                return new RoomForRentAnnouncement(this);
            }
        }
    }
}