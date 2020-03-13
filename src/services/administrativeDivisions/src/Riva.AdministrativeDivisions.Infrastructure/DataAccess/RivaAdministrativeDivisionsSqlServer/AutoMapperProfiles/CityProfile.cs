using AutoMapper;
using Riva.AdministrativeDivisions.Domain.Cities.Aggregates;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities;

namespace Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.AutoMapperProfiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<CityEntity, City>()
                .ForMember(x => x.RowVersion, opt => opt.Ignore())
                .ConstructUsing(x => City.Builder()
                    .SetId(x.Id)
                    .SetRowVersion(x.RowVersion)
                    .SetName(x.Name)
                    .SetPolishName(x.PolishName)
                    .SetStateId(x.StateId)
                    .Build());

            CreateMap<City, CityEntity>();
        }
    }
}