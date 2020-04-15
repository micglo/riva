using AutoMapper;
using Riva.Users.Core.Commands;
using Riva.Users.Core.Queries;
using Riva.Users.Domain.Users.Entities;

namespace Riva.Users.Infrastructure.CoreAutoMapperProfiles
{
    public class RoomForRentAnnouncementPreferenceProfile : Profile
    {
        public RoomForRentAnnouncementPreferenceProfile()
        {
            CreateMap<RoomForRentAnnouncementPreference, RoomForRentAnnouncementPreferenceOutputQuery>();

            CreateMap<CreateRoomForRentAnnouncementPreferenceCommand, RoomForRentAnnouncementPreference>()
                .ForMember(x => x.CityDistricts, opt => opt.Ignore())
                .ConstructUsing(x => RoomForRentAnnouncementPreference.Builder()
                    .SetId(x.RoomForRentAnnouncementPreferenceId)
                    .SetCityId(x.CityId)
                    .SetPriceMin(x.PriceMin)
                    .SetPriceMax(x.PriceMax)
                    .SetRoomType(x.RoomType)
                    .SetCityDistricts(x.CityDistricts)
                    .Build());
        }
    }
}