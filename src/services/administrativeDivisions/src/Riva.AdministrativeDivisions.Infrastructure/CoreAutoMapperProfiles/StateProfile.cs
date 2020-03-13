using System;
using AutoMapper;
using Riva.AdministrativeDivisions.Core.Commands;
using Riva.AdministrativeDivisions.Core.Queries;
using Riva.AdministrativeDivisions.Domain.States.Aggregates;

namespace Riva.AdministrativeDivisions.Infrastructure.CoreAutoMapperProfiles
{
    public class StateProfile : Profile
    {
        public StateProfile()
        {
            CreateMap<State, StateOutputQuery>();

            CreateMap<CreateStateCommand, State>()
                .ConstructUsing(x => State.Builder()
                    .SetId(x.StateId)
                    .SetRowVersion(Array.Empty<byte>())
                    .SetName(x.Name)
                    .SetPolishName(x.PolishName)
                    .Build());
        }
    }
}