using System;
using System.Linq;
using AutoMapper;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Aggregates;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities;

namespace Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.AutoMapperProfiles
{
    public class CityDistrictProfile : Profile
    {
        public CityDistrictProfile()
        {
            CreateMap<CityDistrictEntity, CityDistrict>()
                .ForMember(x => x.RowVersion, opt => opt.Ignore())
                .ForMember(x => x.NameVariants, opt => opt.Ignore())
                .ConvertUsing(x => CityDistrict.Builder()
                    .SetId(x.Id)
                    .SetRowVersion(x.RowVersion)
                    .SetName(x.Name)
                    .SetPolishName(x.PolishName)
                    .SetCityId(x.CityId)
                    .SetParentId(x.ParentId)
                    .SetNameVariants(x.NameVariants.Select(nv => nv.Value).ToArray())
                    .Build());

            CreateMap<CityDistrict, CityDistrictEntity>().ConvertUsing<CityDistrictEntityTypeConverter>();
        }

        private class CityDistrictEntityTypeConverter : ITypeConverter<CityDistrict, CityDistrictEntity>
        {
            public CityDistrictEntity Convert(CityDistrict source, CityDistrictEntity destination, ResolutionContext context)
            {
                return new CityDistrictEntity
                {
                    Id = source.Id,
                    Name = source.Name,
                    PolishName = source.PolishName,
                    CityId = source.CityId,
                    ParentId = source.ParentId,
                    RowVersion = source.RowVersion.ToArray(),
                    NameVariants = source.NameVariants.Select(nv => new CityDistrictNameVariantEntity
                    {
                        Id = Guid.NewGuid(),
                        CityDistrictId = source.Id,
                        Value = nv
                    }).ToList()
                };
            }
        }
    }
}