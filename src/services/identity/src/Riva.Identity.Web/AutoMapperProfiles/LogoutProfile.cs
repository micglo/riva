using AutoMapper;
using Riva.Identity.Core.Interactors.Logout;
using Riva.Identity.Web.Models.ViewModels;

namespace Riva.Identity.Web.AutoMapperProfiles
{
    public class LogoutProfile : Profile
    {
        public LogoutProfile()
        {
            CreateMap<LogoutOutput, LogoutViewModel>();
            CreateMap<LoggedOutOutput, LoggedOutViewModel>();
        }
    }
}