using System;
using System.Collections.Generic;
using AutoMapper;
using Riva.Identity.Domain.Roles.Aggregates;
using Riva.BuildingBlocks.Core.Queries;
using Riva.Identity.Core.Commands;
using Riva.Identity.Core.Queries;

namespace Riva.Identity.Infrastructure.CoreAutoMapperProfiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleOutputQuery>();

            CreateMap<List<Role>, CollectionOutputQuery<RoleOutputQuery>>()
                .ConstructUsing(
                    (x, context) => new CollectionOutputQuery<RoleOutputQuery>(x.Count, context.Mapper.Map<IEnumerable<Role>, IEnumerable<RoleOutputQuery>>(x))
                 );

            CreateMap<CreateRoleCommand, Role>()
                .ConstructUsing(x => new Role(x.RoleId, Array.Empty<byte>(), x.Name));
        }
    }
}