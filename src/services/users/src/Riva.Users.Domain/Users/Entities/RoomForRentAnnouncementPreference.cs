using System;
using System.Collections.Generic;
using Riva.Users.Domain.Users.Builders;
using Riva.Users.Domain.Users.Enumerations;
using Riva.Users.Domain.Users.ValueObjects.EntityValueObjects.RoomForRentAnnouncementPreferenceValueObjects;

namespace Riva.Users.Domain.Users.Entities
{
    public class RoomForRentAnnouncementPreference
    {
        private List<Guid> _cityDistricts;

        public Guid Id { get; }
        public Guid CityId { get; private set; }
        public decimal? PriceMin { get; private set; }
        public decimal? PriceMax { get; private set; }
        public RoomTypeEnumeration RoomType { get; private set; }
        public IReadOnlyCollection<Guid> CityDistricts => _cityDistricts.AsReadOnly();

        private RoomForRentAnnouncementPreference(RoomForRentAnnouncementPreferenceBuilder builder)
        {
            Id = builder.Id;
            CityId = builder.CityId;
            PriceMin = builder.PriceMin;
            PriceMax = builder.PriceMax;
            RoomType = builder.RoomType;
            _cityDistricts = builder.CityDistricts;
        }

        public static IRoomForRentAnnouncementPreferenceIdSetter Builder()
        {
            return new RoomForRentAnnouncementPreferenceBuilder();
        }


        public void ChangeCityId(Guid cityId)
        {
            CityId = new RoomForRentAnnouncementPreferenceCityId(cityId);
        }

        public void ChangePriceMin(decimal? priceMin)
        {
            PriceMin = new RoomForRentAnnouncementPreferencePriceMin(priceMin);
        }

        public void ChangePriceMax(decimal? priceMax)
        {
            PriceMax = new RoomForRentAnnouncementPreferencePriceMax(PriceMin, priceMax);
        }

        public void ChangeRoomType(RoomTypeEnumeration roomType)
        {
            RoomType = roomType;
        }

        public void ChangeCityDistricts(IEnumerable<Guid> cityDistricts)
        {
            _cityDistricts = new RoomForRentAnnouncementPreferenceCityDistricts(cityDistricts);
        }

        private class RoomForRentAnnouncementPreferenceBuilder : IRoomForRentAnnouncementPreferenceIdSetter, IRoomForRentAnnouncementPreferenceCityIdSetter, 
            IRoomForRentAnnouncementPreferenceBuilder
        {
            public Guid Id { get; private set; }
            public Guid CityId { get; private set; }
            public decimal? PriceMin { get; private set; }
            public decimal? PriceMax { get; private set; }
            public RoomTypeEnumeration RoomType { get; private set; }
            public List<Guid> CityDistricts { get; private set; } = new List<Guid>();

            public IRoomForRentAnnouncementPreferenceCityIdSetter SetId(Guid id)
            {
                Id = new RoomForRentAnnouncementPreferenceId(id);
                return this;
            }

            public IRoomForRentAnnouncementPreferenceBuilder SetCityId(Guid cityId)
            {
                CityId = new RoomForRentAnnouncementPreferenceCityId(cityId);
                return this;
            }

            public IRoomForRentAnnouncementPreferenceBuilder SetPriceMin(decimal? priceMin)
            {
                PriceMin = new RoomForRentAnnouncementPreferencePriceMin(priceMin);
                return this;
            }

            public IRoomForRentAnnouncementPreferenceBuilder SetPriceMax(decimal? priceMax)
            {
                PriceMax = new RoomForRentAnnouncementPreferencePriceMax(PriceMin, priceMax);
                return this;
            }

            public IRoomForRentAnnouncementPreferenceBuilder SetRoomType(RoomTypeEnumeration roomType)
            {
                RoomType = roomType;
                return this;
            }

            public IRoomForRentAnnouncementPreferenceBuilder SetCityDistricts(IEnumerable<Guid> cityDistricts)
            {
                CityDistricts = new RoomForRentAnnouncementPreferenceCityDistricts(cityDistricts);
                return this;
            }

            public RoomForRentAnnouncementPreference Build()
            {
                return new RoomForRentAnnouncementPreference(this);
            }
        }
    }
}