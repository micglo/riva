using System;
using System.Collections.Generic;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Enumerations;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Aggregates;

namespace Riva.Announcements.Domain.FlatForRentAnnouncements.Builders
{
    public interface IFlatForRentAnnouncementIdSetter
    {
        IFlatForRentAnnouncementTitleSetter SetId(Guid id);
    }

    public interface IFlatForRentAnnouncementTitleSetter
    {
        IFlatForRentAnnouncementSourceUrlSetter SetTitle(string title);
    }

    public interface IFlatForRentAnnouncementSourceUrlSetter
    {
        IFlatForRentAnnouncementCityIdSetter SetSourceUrl(string sourceUrl);
    }

    public interface IFlatForRentAnnouncementCityIdSetter
    {
        IFlatForRentAnnouncementCreatedSetter SetCityId(Guid cityId);
    }

    public interface IFlatForRentAnnouncementCreatedSetter
    {
        IFlatForRentAnnouncementDescriptionSetter SetCreated(DateTimeOffset created);
    }

    public interface IFlatForRentAnnouncementDescriptionSetter
    {
        IFlatForRentAnnouncementBuilder SetDescription(string description);
    }

    public interface IFlatForRentAnnouncementBuilder
    {
        IFlatForRentAnnouncementBuilder SetPrice(decimal? price);
        IFlatForRentAnnouncementBuilder SetNumberOfRooms(NumberOfRoomsEnumeration numberOfRooms);
        IFlatForRentAnnouncementBuilder SetCityDistricts(IEnumerable<Guid> cityDistricts);
        FlatForRentAnnouncement Build();
    }
}