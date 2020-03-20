using System;
using System.Collections.Generic;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Builders;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Enumerations;
using Riva.Announcements.Domain.FlatForRentAnnouncements.ValueObjects;
using Riva.BuildingBlocks.Domain;

namespace Riva.Announcements.Domain.FlatForRentAnnouncements.Aggregates
{
    public class FlatForRentAnnouncement : AggregateBase
    {
        private readonly List<Guid> _cityDistricts;

        public string Title { get; private set; }
        public string SourceUrl { get; private set; }
        public Guid CityId { get; private set; }
        public DateTimeOffset Created { get; }
        public string Description { get; private set; }
        public decimal? Price { get; private set; }
        public NumberOfRoomsEnumeration NumberOfRooms { get; private set; }
        public IReadOnlyCollection<Guid> CityDistricts => _cityDistricts.AsReadOnly();

        private FlatForRentAnnouncement(FlatForRentAnnouncementBuilder builder) : base(builder.Id)
        {
            Title = builder.Title;
            SourceUrl = builder.SourceUrl;
            CityId = builder.CityId;
            Created = builder.Created;
            Description = builder.Description;
            Price = builder.Price;
            NumberOfRooms = builder.NumberOfRooms;
            _cityDistricts = builder.CityDistricts;
        }

        public static IFlatForRentAnnouncementIdSetter Builder()
        {
            return new FlatForRentAnnouncementBuilder();
        }

        public void ChangeTitle(string title)
        {
            Title = new FlatForRentAnnouncementTitle(title);
        }

        public void ChangeSourceUrl(string sourceUrl)
        {
            SourceUrl = new FlatForRentAnnouncementSourceUrl(sourceUrl);
        }

        public void ChangeCityId(Guid cityId)
        {
            CityId = new FlatForRentAnnouncementCityId(cityId);
        }

        public void ChangeDescription(string description)
        {
            Description = new FlatForRentAnnouncementDescription(description);
        }

        public void ChangePrice(decimal? price)
        {
            Price = price;
        }

        public void ChangeNumberOfRooms(NumberOfRoomsEnumeration numberOfRooms)
        {
            NumberOfRooms = numberOfRooms;
        }

        public void AddCityDistrict(Guid cityDistrict)
        {
            cityDistrict = new FlatForRentAnnouncementCityDistrict(cityDistrict);
            var anyDuplicates = _cityDistricts.Contains(cityDistrict);
            if (!anyDuplicates)
                _cityDistricts.Add(cityDistrict);
        }

        public void RemoveCityDistrict(Guid cityDistrict)
        {
            _cityDistricts.Remove(new FlatForRentAnnouncementCityDistrict(cityDistrict));
        }

        private class FlatForRentAnnouncementBuilder : IFlatForRentAnnouncementIdSetter, IFlatForRentAnnouncementTitleSetter, 
            IFlatForRentAnnouncementSourceUrlSetter, IFlatForRentAnnouncementCityIdSetter, IFlatForRentAnnouncementCreatedSetter, 
            IFlatForRentAnnouncementDescriptionSetter, IFlatForRentAnnouncementBuilder
        {
            public Guid Id { get; private set; }
            public string Title { get; private set; }
            public string SourceUrl { get; private set; }
            public Guid CityId { get; private set; }
            public DateTimeOffset Created { get; private set; }
            public string Description { get; private set; }
            public decimal? Price { get; private set; }
            public NumberOfRoomsEnumeration NumberOfRooms { get; private set; }
            public List<Guid> CityDistricts { get; private set; } = new List<Guid>();

            public IFlatForRentAnnouncementTitleSetter SetId(Guid id)
            {
                Id = id;
                return this;
            }

            public IFlatForRentAnnouncementSourceUrlSetter SetTitle(string title)
            {
                Title = new FlatForRentAnnouncementTitle(title);
                return this;
            }

            public IFlatForRentAnnouncementCityIdSetter SetSourceUrl(string sourceUrl)
            {
                SourceUrl = new FlatForRentAnnouncementSourceUrl(sourceUrl);
                return this;
            }

            public IFlatForRentAnnouncementCreatedSetter SetCityId(Guid cityId)
            {
                CityId = new FlatForRentAnnouncementCityId(cityId);
                return this;
            }

            public IFlatForRentAnnouncementDescriptionSetter SetCreated(DateTimeOffset created)
            {
                Created = created;
                return this;
            }

            public IFlatForRentAnnouncementBuilder SetDescription(string description)
            {
                Description = new FlatForRentAnnouncementDescription(description);
                return this;
            }

            public IFlatForRentAnnouncementBuilder SetNumberOfRooms(NumberOfRoomsEnumeration numberOfRooms)
            {
                NumberOfRooms = numberOfRooms;
                return this;
            }

            public IFlatForRentAnnouncementBuilder SetPrice(decimal? price)
            {
                Price = price;
                return this;
            }

            public IFlatForRentAnnouncementBuilder SetCityDistricts(IEnumerable<Guid> cityDistricts)
            {
                CityDistricts = new FlatForRentAnnouncementCityDistricts(cityDistricts);
                return this;
            }

            public FlatForRentAnnouncement Build()
            {
                return new FlatForRentAnnouncement(this);
            }
        }
    }
}