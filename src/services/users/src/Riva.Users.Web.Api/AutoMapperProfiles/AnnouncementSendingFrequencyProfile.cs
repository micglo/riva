using AutoMapper;
using Riva.Users.Core.Enums;
using Riva.Users.Core.Extensions;
using Riva.Users.Domain.Users.Enumerations;

namespace Riva.Users.Web.Api.AutoMapperProfiles
{
    public class AnnouncementSendingFrequencyProfile : Profile
    {
        public AnnouncementSendingFrequencyProfile()
        {
            CreateMap<AnnouncementSendingFrequencyEnumeration, AnnouncementSendingFrequency>()
                .ConvertUsing(x => x.ConvertToEnum());
        }
    }
}