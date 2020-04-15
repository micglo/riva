using AutoMapper;
using Riva.Users.Core.Extensions;
using Riva.Users.Core.Queries;
using Riva.Users.Web.Api.Models.Responses;

namespace Riva.Users.Web.Api.AutoMapperProfiles
{
    public class RoomForRentAnnouncementPreferenceProfile : Profile
    {
        public RoomForRentAnnouncementPreferenceProfile()
        {
            CreateMap<RoomForRentAnnouncementPreferenceOutputQuery, RoomForRentAnnouncementPreferenceResponse>()
                .ConvertUsing(x => new RoomForRentAnnouncementPreferenceResponse(x.Id, x.CityId, x.PriceMin, x.PriceMax, x.RoomType.ConvertToEnum(), x.CityDistricts));
        }
    }
}