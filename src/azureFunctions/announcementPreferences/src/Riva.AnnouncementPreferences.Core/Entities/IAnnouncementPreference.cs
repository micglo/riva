using System;
using System.Collections.Generic;
using Cosmonaut;
using Riva.AnnouncementPreferences.Core.Enums;

namespace Riva.AnnouncementPreferences.Core.Entities
{
    public interface IAnnouncementPreference : ISharedCosmosEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserEmail { get; set; }
        public Guid CityId { get; set; }
        public bool ServiceActive { get; set; }
        public AnnouncementSendingFrequency AnnouncementSendingFrequency { get; set; }
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }
        public List<Guid> CityDistricts { get; set; }
        public List<string> AnnouncementUrlsToSend { get; set; }
    }
}