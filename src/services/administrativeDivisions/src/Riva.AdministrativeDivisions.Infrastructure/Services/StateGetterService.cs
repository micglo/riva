using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Riva.AdministrativeDivisions.Core.Enumerations;
using Riva.AdministrativeDivisions.Core.ErrorMessages;
using Riva.AdministrativeDivisions.Core.Services;
using Riva.AdministrativeDivisions.Domain.States.Aggregates;
using Riva.AdministrativeDivisions.Domain.States.Repositories;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.AdministrativeDivisions.Infrastructure.Services
{
    public class StateGetterService : IStateGetterService
    {
        private readonly IStateRepository _stateRepository;

        public StateGetterService(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        public async Task<GetResult<State>> GetByIdAsync(Guid id)
        {
            var state = await _stateRepository.GetByIdAsync(id);
            if (state is null)
            {
                var errors = new Collection<IError>
                {
                    new Error(StateErrorCodeEnumeration.NotFound, StateErrorMessage.NotFound)
                };
                return GetResult<State>.Fail(errors);
            }

            return GetResult<State>.Ok(state);
        }
    }
}