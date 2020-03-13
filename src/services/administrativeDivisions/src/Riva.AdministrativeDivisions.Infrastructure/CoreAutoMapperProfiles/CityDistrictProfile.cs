using System;
using System.Linq;
using AutoMapper;
using Riva.AdministrativeDivisions.Core.Commands;
using Riva.AdministrativeDivisions.Core.Queries;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Aggregates;

namespace Riva.AdministrativeDivisions.Infrastructure.CoreAutoMapperProfiles
{
    public class CityDistrictProfile : Profile
    {
        public CityDistrictProfile()
        {
            CreateMap<CityDistrict, CityDistrictOutputQuery>();

            CreateMap<CreateCityDistrictCommand, CityDistrict>()
                .ForMember(x => x.NameVariants, opt => opt.Ignore())
                .ConstructUsing(x =>
                    CityDistrict
                        .Builder()
                        .SetId(x.CityDistrictId)
                        .SetRowVersion(Array.Empty<byte>())
                        .SetName(x.Name)
                        .SetPolishName(x.PolishName)
                        .SetCityId(x.CityId)
                        .SetParentId(x.ParentId)
                        .SetNameVariants(x.NameVariants.ToArray())
                        .Build());
        }
    }
}