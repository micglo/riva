using AutoMapper;
using Riva.AdministrativeDivisions.Domain.States.Aggregates;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Entities;

namespace Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.AutoMapperProfiles
{
    public class StateProfile : Profile
    {
        public StateProfile()
        {
            CreateMap<StateEntity, State>()
                .ForMember(x => x.RowVersion, opt => opt.Ignore())
                .ConstructUsing(x => State.Builder()
                    .SetId(x.Id)
                    .SetRowVersion(x.RowVersion)
                    .SetName(x.Name)
                    .SetPolishName(x.PolishName)
                    .Build());

            CreateMap<State, StateEntity>();
        }
    }
}