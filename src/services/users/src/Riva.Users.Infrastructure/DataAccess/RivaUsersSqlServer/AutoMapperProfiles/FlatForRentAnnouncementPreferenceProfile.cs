using System.Linq;
using AutoMapper;
using Riva.Users.Domain.Users.Entities;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Entities;

namespace Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.AutoMapperProfiles
{
    public class FlatForRentAnnouncementPreferenceProfile : Profile
    {
        public FlatForRentAnnouncementPreferenceProfile()
        {
            CreateMap<AnnouncementPreferenceEntity, FlatForRentAnnouncementPreference>()
                .ConvertUsing(x => FlatForRentAnnouncementPreference.Builder()
                    .SetId(x.Id)
                    .SetCityId(x.CityId)
                    .SetPriceMin(x.PriceMin)
                    .SetPriceMax(x.PriceMax)
                    .SetRoomNumbersMin(x.RoomNumbersMin)
                    .SetRoomNumbersMax(x.RoomNumbersMax)
                    .SetCityDistricts(x.AnnouncementPreferenceCityDistricts.Select(acd => acd.CityDistrictId))
                    .Build());
        }
    }
}