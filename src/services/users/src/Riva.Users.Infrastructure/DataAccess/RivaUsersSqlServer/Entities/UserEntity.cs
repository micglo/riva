using System.Collections.Generic;
using Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Models;
using Riva.Users.Core.Enums;

namespace Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Entities
{
    public class UserEntity : EntityBase
    {
        public string Email { get; set; }
        public string Picture { get; set; }
        public bool ServiceActive { get; set; }
        public int AnnouncementPreferenceLimit { get; set; }
        public AnnouncementSendingFrequency AnnouncementSendingFrequency { get; set; }
        public ICollection<AnnouncementPreferenceEntity> AnnouncementPreferences { get; set; }

        public UserEntity()
        {
            AnnouncementPreferences = new List<AnnouncementPreferenceEntity>();
        }
    }
}