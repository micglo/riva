using System.Threading;
using System.Threading.Tasks;
using Riva.AdministrativeDivisions.Core.Services;
using Riva.AdministrativeDivisions.Domain.States.Aggregates;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.AdministrativeDivisions.Core.Queries.Handlers
{
    public class GetStateQueryHandler : IQueryHandler<GetStateInputQuery, StateOutputQuery>
    {
        private readonly IStateGetterService _stateGetterService;
        private readonly IMapper _mapper;

        public GetStateQueryHandler(IStateGetterService stateGetterService, IMapper mapper)
        {
            _stateGetterService = stateGetterService;
            _mapper = mapper;
        }

        public async Task<StateOutputQuery> HandleAsync(GetStateInputQuery inputQuery, CancellationToken cancellationToken = default)
        {
            var getStateResult = await _stateGetterService.GetByIdAsync(inputQuery.StateId);
            if(getStateResult.Success)
                return _mapper.Map<State, StateOutputQuery>(getStateResult.Value);
            throw new ResourceNotFoundException(getStateResult.Errors);
        }
    }
}