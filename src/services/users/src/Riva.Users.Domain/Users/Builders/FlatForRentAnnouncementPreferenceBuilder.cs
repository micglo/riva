using System;
using System.Collections.Generic;
using Riva.Users.Domain.Users.Entities;

namespace Riva.Users.Domain.Users.Builders
{
    public interface IFlatForRentAnnouncementPreferenceIdSetter
    {
        IFlatForRentAnnouncementPreferenceCityIdSetter SetId(Guid id);
    }

    public interface IFlatForRentAnnouncementPreferenceCityIdSetter
    {
        IFlatForRentAnnouncementPreferencePriceMinSetter SetCityId(Guid cityId);
    }

    public interface IFlatForRentAnnouncementPreferencePriceMinSetter
    {
        IFlatForRentAnnouncementPreferencePriceMaxSetter SetPriceMin(decimal? priceMin);
    }

    public interface IFlatForRentAnnouncementPreferencePriceMaxSetter
    {
        IFlatForRentAnnouncementPreferenceRoomNumbersMinSetter SetPriceMax(decimal? priceMax);
    }

    public interface IFlatForRentAnnouncementPreferenceRoomNumbersMinSetter
    {
        IFlatForRentAnnouncementPreferenceRoomNumbersMaxSetter SetRoomNumbersMin(int? roomNumbersMin);
    }

    public interface IFlatForRentAnnouncementPreferenceRoomNumbersMaxSetter
    {
        IFlatForRentAnnouncementPreferenceBuilder SetRoomNumbersMax(int? roomNumbersMax);
    }

    public interface IFlatForRentAnnouncementPreferenceBuilder
    {
        IFlatForRentAnnouncementPreferenceBuilder SetCityDistricts(IEnumerable<Guid> cityDistricts);
        FlatForRentAnnouncementPreference Build();
    }
}