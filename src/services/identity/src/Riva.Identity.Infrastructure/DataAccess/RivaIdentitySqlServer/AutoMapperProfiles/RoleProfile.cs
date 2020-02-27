using AutoMapper;
using Riva.Identity.Domain.Roles.Aggregates;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Entities;

namespace Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.AutoMapperProfiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleEntity, Role>();
            CreateMap<Role, RoleEntity>();
        }
    }
}