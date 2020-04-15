using System.Linq;
using AutoMapper;
using Riva.Users.Core.Extensions;
using Riva.Users.Domain.Users.Entities;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Entities;

namespace Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.AutoMapperProfiles
{
    public class RoomForRentAnnouncementPreferenceProfile : Profile
    {
        public RoomForRentAnnouncementPreferenceProfile()
        {
            CreateMap<AnnouncementPreferenceEntity, RoomForRentAnnouncementPreference>()
                .ConvertUsing(x => RoomForRentAnnouncementPreference.Builder()
                    .SetId(x.Id)
                    .SetCityId(x.CityId)
                    .SetPriceMin(x.PriceMin)
                    .SetPriceMax(x.PriceMax)
                    .SetRoomType(x.RoomType.ConvertToEnumeration())
                    .SetCityDistricts(x.AnnouncementPreferenceCityDistricts.Select(acd => acd.CityDistrictId))
                    .Build());
        }
    }
}