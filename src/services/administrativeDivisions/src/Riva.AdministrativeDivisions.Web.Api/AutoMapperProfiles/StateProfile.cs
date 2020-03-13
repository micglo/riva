using System;
using System.Collections.Generic;
using AutoMapper;
using Riva.AdministrativeDivisions.Core.Commands;
using Riva.AdministrativeDivisions.Core.Queries;
using Riva.AdministrativeDivisions.Web.Api.Models.Requests;
using Riva.AdministrativeDivisions.Web.Api.Models.Responses;
using Riva.BuildingBlocks.Core.Queries;
using Riva.BuildingBlocks.WebApi.Models.Responses;

namespace Riva.AdministrativeDivisions.Web.Api.AutoMapperProfiles
{
    public class StateProfile : Profile
    {
        public StateProfile()
        {
            CreateMap<StateOutputQuery, StateResponse>();

            CreateMap<GetStatesRequest, GetStatesInputQuery>();

            CreateMap<CollectionOutputQuery<StateOutputQuery>, CollectionResponse<StateResponse>>()
                .ForMember(x => x.Results, opt => opt.Ignore())
                .ConstructUsing((x, context) => new CollectionResponse<StateResponse>(x.TotalCount,
                    context.Mapper.Map<IEnumerable<StateOutputQuery>, IEnumerable<StateResponse>>(x.Results)));

            CreateMap<CreateStateRequest, CreateStateCommand>()
                .ConstructUsing(x => new CreateStateCommand(Guid.NewGuid(), x.Name, x.PolishName));

            CreateMap<UpdateStateRequest, UpdateStateCommand>()
                .ForMember(x => x.RowVersion, opt => opt.Ignore())
                .ConstructUsing(x => new UpdateStateCommand(x.Id, x.RowVersion, x.Name, x.PolishName));
        }
    }
}