using System.Linq;
using AutoMapper;
using Riva.Users.Domain.Cities.Aggregates;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Entities;

namespace Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.AutoMapperProfiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<CityEntity, City>()
                .ForMember(x => x.CityDistricts, opt => opt.Ignore())
                .ConstructUsing(x => new City(x.Id, x.CityDistricts.Select(cd => cd.Id)));
        }
    }
}