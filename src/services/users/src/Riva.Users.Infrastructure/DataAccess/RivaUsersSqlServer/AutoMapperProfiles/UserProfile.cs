using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Riva.Users.Core.Enums;
using Riva.Users.Domain.Users.Aggregates;
using Riva.Users.Domain.Users.Entities;
using Riva.Users.Domain.Users.Enumerations;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Entities;

namespace Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.AutoMapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserEntity, User>()
                .ConstructUsing((entity, context) => User.Builder()
                    .SetId(entity.Id)
                    .SetEmail(entity.Email)
                    .SetServiceActive(entity.ServiceActive)
                    .SetAnnouncementPreferenceLimit(entity.AnnouncementPreferenceLimit)
                    .SetAnnouncementSendingFrequency(context.Mapper.Map<AnnouncementSendingFrequency, AnnouncementSendingFrequencyEnumeration>(entity.AnnouncementSendingFrequency))
                    .SetPicture(entity.Picture)
                    .SetFlatForRentAnnouncementPreferences(context.Mapper.Map<ICollection<AnnouncementPreferenceEntity>, IEnumerable<FlatForRentAnnouncementPreference>>(entity.AnnouncementPreferences.Where(x => x.AnnouncementPreferenceType == AnnouncementPreferenceType.FlatForRentAnnouncementPreference).ToList()))
                    .SetRoomForRentAnnouncementPreferences(context.Mapper.Map<ICollection<AnnouncementPreferenceEntity>, IEnumerable<RoomForRentAnnouncementPreference>>(entity.AnnouncementPreferences.Where(x => x.AnnouncementPreferenceType == AnnouncementPreferenceType.RoomForRentAnnouncementPreference).ToList()))
                    .Build());

            CreateMap<User, UserEntity>()
                .ConstructUsing((user, context) => new UserEntity
                {
                    Id = user.Id,
                    Email = user.Email,
                    Picture = user.Picture,
                    ServiceActive = user.ServiceActive,
                    AnnouncementPreferenceLimit = user.AnnouncementPreferenceLimit,
                    AnnouncementSendingFrequency = context.Mapper.Map<AnnouncementSendingFrequencyEnumeration, AnnouncementSendingFrequency>(user.AnnouncementSendingFrequency)
                });
        }
    }
}