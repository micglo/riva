using AutoMapper;
using Riva.Users.Core.Commands;
using Riva.Users.Core.Queries;
using Riva.Users.Domain.Users.Entities;

namespace Riva.Users.Infrastructure.CoreAutoMapperProfiles
{
    public class FlatForRentAnnouncementPreferenceProfile : Profile
    {
        public FlatForRentAnnouncementPreferenceProfile()
        {
            CreateMap<FlatForRentAnnouncementPreference, FlatForRentAnnouncementPreferenceOutputQuery>();

            CreateMap<CreateFlatForRentAnnouncementPreferenceCommand, FlatForRentAnnouncementPreference>()
                .ForMember(x => x.CityDistricts, opt => opt.Ignore())
                .ConstructUsing(x => FlatForRentAnnouncementPreference.Builder()
                    .SetId(x.FlatForRentAnnouncementPreferenceId)
                    .SetCityId(x.CityId)
                    .SetPriceMin(x.PriceMin)
                    .SetPriceMax(x.PriceMax)
                    .SetRoomNumbersMin(x.RoomNumbersMin)
                    .SetRoomNumbersMax(x.RoomNumbersMax)
                    .SetCityDistricts(x.CityDistricts)
                    .Build());
        }
    }
}