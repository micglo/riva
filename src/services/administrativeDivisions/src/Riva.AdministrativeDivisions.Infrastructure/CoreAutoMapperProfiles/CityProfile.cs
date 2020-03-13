using System;
using AutoMapper;
using Riva.AdministrativeDivisions.Core.Commands;
using Riva.AdministrativeDivisions.Core.Queries;
using Riva.AdministrativeDivisions.Domain.Cities.Aggregates;

namespace Riva.AdministrativeDivisions.Infrastructure.CoreAutoMapperProfiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<City, CityOutputQuery>();

            CreateMap<CreateCityCommand, City>()
                .ConstructUsing(x =>
                    City.Builder()
                        .SetId(x.CityId)
                        .SetRowVersion(Array.Empty<byte>())
                        .SetName(x.Name)
                        .SetPolishName(x.PolishName)
                        .SetStateId(x.StateId)
                        .Build());
        }
    }
}