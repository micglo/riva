using System;
using System.Collections.Generic;
using Cosmonaut.Attributes;
using Riva.AnnouncementPreferences.Core.Constants;
using Riva.AnnouncementPreferences.Core.Enums;

namespace Riva.AnnouncementPreferences.Core.Entities
{
    [SharedCosmosCollection(ConstantVariables.CosmosCollectionName, nameof(RoomForRentAnnouncementPreference))]
    public class RoomForRentAnnouncementPreference : IAnnouncementPreference
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserEmail { get; set; }
        public Guid CityId { get; set; }
        public bool ServiceActive { get; set; }
        public AnnouncementSendingFrequency AnnouncementSendingFrequency { get; set; }
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }
        public RoomType? RoomType { get; set; }
        public List<Guid> CityDistricts { get; set; } = new List<Guid>();
        public List<string> AnnouncementUrlsToSend { get; set; } = new List<string>();
        public string CosmosEntityName { get; set; } = nameof(RoomForRentAnnouncementPreference);
    }
}