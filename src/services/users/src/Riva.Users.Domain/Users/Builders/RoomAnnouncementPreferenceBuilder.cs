using System;
using System.Collections.Generic;
using Riva.Users.Domain.Users.Entities;
using Riva.Users.Domain.Users.Enumerations;

namespace Riva.Users.Domain.Users.Builders
{
    public interface IRoomForRentAnnouncementPreferenceIdSetter
    {
        IRoomForRentAnnouncementPreferenceCityIdSetter SetId(Guid id);
    }

    public interface IRoomForRentAnnouncementPreferenceCityIdSetter
    {
        IRoomForRentAnnouncementPreferenceBuilder SetCityId(Guid cityId);
    }

    public interface IRoomForRentAnnouncementPreferenceBuilder
    {
        IRoomForRentAnnouncementPreferenceBuilder SetPriceMin(decimal? priceMin);
        IRoomForRentAnnouncementPreferenceBuilder SetPriceMax(decimal? priceMax);
        IRoomForRentAnnouncementPreferenceBuilder SetRoomType(RoomTypeEnumeration roomType);
        IRoomForRentAnnouncementPreferenceBuilder SetCityDistricts(IEnumerable<Guid> cityDistricts);
        RoomForRentAnnouncementPreference Build();
    }
}