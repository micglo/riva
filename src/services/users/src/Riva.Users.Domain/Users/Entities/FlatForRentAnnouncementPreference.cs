using System;
using System.Collections.Generic;
using Riva.Users.Domain.Users.Builders;
using Riva.Users.Domain.Users.ValueObjects.EntityValueObjects.FlatForRentAnnouncementPreferenceValueObjects;

namespace Riva.Users.Domain.Users.Entities
{
    public class FlatForRentAnnouncementPreference
    {
        private List<Guid> _cityDistricts;

        public Guid Id { get; }
        public Guid CityId { get; private set; }
        public decimal? PriceMin { get; private set; }
        public decimal? PriceMax { get; private set; }
        public int? RoomNumbersMin { get; private set; }
        public int? RoomNumbersMax { get; private set; }
        public IReadOnlyCollection<Guid> CityDistricts => _cityDistricts.AsReadOnly();

        private FlatForRentAnnouncementPreference(FlatForRentAnnouncementPreferenceBuilder builder)
        {
            Id = builder.Id;
            CityId = builder.CityId;
            PriceMin = builder.PriceMin;
            PriceMax = builder.PriceMax;
            RoomNumbersMin = builder.RoomNumbersMin;
            RoomNumbersMax = builder.RoomNumbersMax;
            _cityDistricts = builder.CityDistricts;
        }

        public static IFlatForRentAnnouncementPreferenceIdSetter Builder()
        {
            return new FlatForRentAnnouncementPreferenceBuilder();
        }

        public void ChangeCityId(Guid cityId)
        {
            CityId = new FlatForRentAnnouncementPreferenceCityId(cityId);
        }

        public void ChangePriceMin(decimal? priceMin)
        {
            PriceMin = new FlatForRentAnnouncementPreferencePriceMin(priceMin);
        }

        public void ChangePriceMax(decimal? priceMax)
        {
            PriceMax = new FlatForRentAnnouncementPreferencePriceMax(PriceMin, priceMax);
        }

        public void ChangeRoomNumbersMin(int? roomNumbersMin)
        {
            RoomNumbersMin = new FlatForRentAnnouncementPreferenceRoomNumbersMin(roomNumbersMin);
        }

        public void ChangeRoomNumbersMax(int? roomNumbersMax)
        {
            RoomNumbersMax = new FlatForRentAnnouncementPreferenceRoomNumbersMax(RoomNumbersMin, roomNumbersMax);
        }

        public void ChangeCityDistricts(IEnumerable<Guid> cityDistricts)
        {
            _cityDistricts = new FlatForRentAnnouncementPreferenceCityDistricts(cityDistricts);
        }

        private class FlatForRentAnnouncementPreferenceBuilder : IFlatForRentAnnouncementPreferenceIdSetter, IFlatForRentAnnouncementPreferenceCityIdSetter, 
            IFlatForRentAnnouncementPreferencePriceMinSetter, IFlatForRentAnnouncementPreferencePriceMaxSetter, IFlatForRentAnnouncementPreferenceRoomNumbersMinSetter, 
            IFlatForRentAnnouncementPreferenceRoomNumbersMaxSetter, IFlatForRentAnnouncementPreferenceBuilder
        {
            public Guid Id { get; private set; }
            public Guid CityId { get; private set; }
            public decimal? PriceMin { get; private set; }
            public decimal? PriceMax { get; private set; }
            public int? RoomNumbersMin { get; private set; }
            public int? RoomNumbersMax { get; private set; }
            public List<Guid> CityDistricts { get; private set; } = new List<Guid>();

            public IFlatForRentAnnouncementPreferenceCityIdSetter SetId(Guid id)
            {
                Id = new FlatForRentAnnouncementPreferenceId(id);
                return this;
            }

            public IFlatForRentAnnouncementPreferencePriceMinSetter SetCityId(Guid cityId)
            {
                CityId = new FlatForRentAnnouncementPreferenceCityId(cityId);
                return this;
            }

            public IFlatForRentAnnouncementPreferencePriceMaxSetter SetPriceMin(decimal? priceMin)
            {
                PriceMin = new FlatForRentAnnouncementPreferencePriceMin(priceMin);
                return this;
            }

            public IFlatForRentAnnouncementPreferenceRoomNumbersMinSetter SetPriceMax(decimal? priceMax)
            {
                PriceMax = new FlatForRentAnnouncementPreferencePriceMax(PriceMin, priceMax);
                return this;
            }

            public IFlatForRentAnnouncementPreferenceRoomNumbersMaxSetter SetRoomNumbersMin(int? roomNumbersMin)
            {
                RoomNumbersMin = new FlatForRentAnnouncementPreferenceRoomNumbersMin(roomNumbersMin);
                return this;
            }

            public IFlatForRentAnnouncementPreferenceBuilder SetRoomNumbersMax(int? roomNumbersMax)
            {
                RoomNumbersMax = new FlatForRentAnnouncementPreferenceRoomNumbersMax(RoomNumbersMin, roomNumbersMax);
                return this;
            }

            public IFlatForRentAnnouncementPreferenceBuilder SetCityDistricts(IEnumerable<Guid> cityDistricts)
            {
                CityDistricts = new FlatForRentAnnouncementPreferenceCityDistricts(cityDistricts);
                return this;
            }

            public FlatForRentAnnouncementPreference Build()
            {
                return new FlatForRentAnnouncementPreference(this);
            }
        }
    }
}