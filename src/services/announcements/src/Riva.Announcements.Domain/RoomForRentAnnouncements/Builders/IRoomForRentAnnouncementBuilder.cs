using System;
using System.Collections.Generic;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Aggregates;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Enumerations;

namespace Riva.Announcements.Domain.RoomForRentAnnouncements.Builders
{
    public interface IRoomForRentAnnouncementIdSetter
    {
        IRoomForRentAnnouncementTitleSetter SetId(Guid id);
    }

    public interface IRoomForRentAnnouncementTitleSetter
    {
        IRoomForRentAnnouncementSourceUrlSetter SetTitle(string title);
    }

    public interface IRoomForRentAnnouncementSourceUrlSetter
    {
        IRoomForRentAnnouncementCityIdSetter SetSourceUrl(string sourceUrl);
    }

    public interface IRoomForRentAnnouncementCityIdSetter
    {
        IRoomForRentAnnouncementCreatedSetter SetCityId(Guid cityId);
    }

    public interface IRoomForRentAnnouncementCreatedSetter
    {
        IRoomForRentAnnouncementDescriptionSetter SetCreated(DateTimeOffset created);
    }

    public interface IRoomForRentAnnouncementDescriptionSetter
    {
        IRoomForRentAnnouncementBuilder SetDescription(string description);
    }

    public interface IRoomForRentAnnouncementBuilder
    {
        IRoomForRentAnnouncementBuilder SetPrice(decimal? price);
        IRoomForRentAnnouncementBuilder SetRoomTypes(IEnumerable<RoomTypeEnumeration> roomTypes);
        IRoomForRentAnnouncementBuilder SetCityDistricts(IEnumerable<Guid> cityDistricts);
        RoomForRentAnnouncement Build();
    }
}