using System.Collections.Generic;
using AutoMapper;
using Riva.Users.Core.Commands;
using Riva.Users.Core.Queries;
using Riva.Users.Domain.Users.Aggregates;
using Riva.Users.Domain.Users.Entities;

namespace Riva.Users.Infrastructure.CoreAutoMapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserOutputQuery>()
                .ForMember(x => x.FlatForRentAnnouncementPreferences, opt => opt.Ignore())
                .ForMember(x => x.RoomForRentAnnouncementPreferences, opt => opt.Ignore())
                .ConstructUsing((user, context) => new UserOutputQuery(user.Id, user.Email, user.Picture,
                    user.ServiceActive, user.AnnouncementPreferenceLimit, user.AnnouncementSendingFrequency,
                    context.Mapper
                        .Map<IReadOnlyCollection<RoomForRentAnnouncementPreference>,
                            IEnumerable<RoomForRentAnnouncementPreferenceOutputQuery>>(
                            user.RoomForRentAnnouncementPreferences),
                    context.Mapper
                        .Map<IReadOnlyCollection<FlatForRentAnnouncementPreference>,
                            IEnumerable<FlatForRentAnnouncementPreferenceOutputQuery>>(
                            user.FlatForRentAnnouncementPreferences)));

            CreateMap<CreateUserCommand, User>()
                .ConstructUsing(x => User.Builder()
                    .SetId(x.UserId)
                    .SetEmail(x.Email)
                    .SetServiceActive(x.ServiceActive)
                    .SetAnnouncementPreferenceLimit(x.AnnouncementPreferenceLimit)
                    .SetAnnouncementSendingFrequency(x.AnnouncementSendingFrequency)
                    .Build());
        }
    }
}