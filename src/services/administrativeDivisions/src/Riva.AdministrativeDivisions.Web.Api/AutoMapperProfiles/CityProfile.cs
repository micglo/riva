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
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<CityOutputQuery, CityResponse>();

            CreateMap<GetCitiesRequest, GetCitiesInputQuery>();

            CreateMap<CollectionOutputQuery<CityOutputQuery>, CollectionResponse<CityResponse>>()
                .ForMember(x => x.Results, opt => opt.Ignore())
                .ConstructUsing((x, context) => new CollectionResponse<CityResponse>(x.TotalCount,
                    context.Mapper.Map<IEnumerable<CityOutputQuery>, IEnumerable<CityResponse>>(x.Results)));

            CreateMap<CreateCityRequest, CreateCityCommand>()
                .ConstructUsing(x => new CreateCityCommand(Guid.NewGuid(), x.Name, x.PolishName, x.StateId));

            CreateMap<UpdateCityRequest, UpdateCityCommand>()
                .ForMember(x => x.RowVersion, opt => opt.Ignore())
                .ConstructUsing(x => new UpdateCityCommand(x.Id, x.RowVersion, x.Name, x.PolishName, x.StateId));
        }
    }
}