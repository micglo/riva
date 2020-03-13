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
    public class CityDistrictDistrictProfile : Profile
    {
        public CityDistrictDistrictProfile()
        {
            CreateMap<CityDistrictOutputQuery, CityDistrictResponse>();

            CreateMap<GetCityDistrictsRequest, GetCityDistrictsInputQuery>();

            CreateMap<CollectionOutputQuery<CityDistrictOutputQuery>, CollectionResponse<CityDistrictResponse>>()
                .ForMember(x => x.Results, opt => opt.Ignore())
                .ConstructUsing((x, context) => new CollectionResponse<CityDistrictResponse>(x.TotalCount,
                    context.Mapper.Map<IEnumerable<CityDistrictOutputQuery>, IEnumerable<CityDistrictResponse>>(x.Results)));

            CreateMap<CreateCityDistrictRequest, CreateCityDistrictCommand>()
                .ForMember(x => x.NameVariants, opt => opt.Ignore())
                .ConstructUsing(x => new CreateCityDistrictCommand(Guid.NewGuid(), x.Name, x.PolishName, x.CityId, x.ParentId, x.NameVariants));

            CreateMap<UpdateCityDistrictRequest, UpdateCityDistrictCommand>()
                .ForMember(x => x.RowVersion, opt => opt.Ignore())
                .ForMember(x => x.NameVariants, opt => opt.Ignore())
                .ConstructUsing(x => new UpdateCityDistrictCommand(x.Id, x.RowVersion, x.Name, x.PolishName, x.CityId,
                    x.ParentId, x.NameVariants));
        }
    }
}