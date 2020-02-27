using System;
using System.Collections.Generic;
using AutoMapper;
using Riva.BuildingBlocks.Core.Queries;
using Riva.BuildingBlocks.WebApi.Models.Responses;
using Riva.Identity.Core.Commands;
using Riva.Identity.Core.Queries;
using Riva.Identity.Web.Api.Models.Requests.Roles;
using Riva.Identity.Web.Api.Models.Responses.Roles;

namespace Riva.Identity.Web.Api.AutoMapperProfiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleOutputQuery, GetRoleResponse>();

            CreateMap<CollectionOutputQuery<RoleOutputQuery>, CollectionResponse<GetRoleResponse>>()
                .ForMember(x => x.Results, opt => opt.Ignore())
                .ConstructUsing(
                    (x, context) => 
                        new CollectionResponse<GetRoleResponse>(x.TotalCount, context.Mapper.Map<IEnumerable<RoleOutputQuery>, IEnumerable<GetRoleResponse>>(x.Results))
                    );

            
            CreateMap<CreateRoleRequest, CreateRoleCommand>()
                .ConstructUsing(x => new CreateRoleCommand(Guid.NewGuid(), x.Name));

            CreateMap<UpdateRoleRequest, UpdateRoleCommand>()
                .ForMember(x => x.RowVersion, opt => opt.Ignore())
                .ConstructUsing(x => new UpdateRoleCommand(x.Id, x.RowVersion, x.Name));
        }
    }
}