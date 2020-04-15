using AutoMapper;
using Riva.Users.Core.Queries;
using Riva.Users.Web.Api.Models.Responses;

namespace Riva.Users.Web.Api.AutoMapperProfiles
{
    public class FlatForRentAnnouncementPreferenceProfile : Profile
    {
        public FlatForRentAnnouncementPreferenceProfile()
        {
            CreateMap<FlatForRentAnnouncementPreferenceOutputQuery, FlatForRentAnnouncementPreferenceResponse>()
                .ConvertUsing(x => new FlatForRentAnnouncementPreferenceResponse(x.Id, x.CityId, x.PriceMin, x.PriceMax, x.RoomNumbersMin, x.RoomNumbersMax, x.CityDistricts));
        }
    }
}