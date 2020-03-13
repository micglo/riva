using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Riva.AdministrativeDivisions.Domain.States.Aggregates;
using Riva.AdministrativeDivisions.Domain.States.Repositories;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.AdministrativeDivisions.Core.Queries.Handlers
{
    public class GetStatesQueryHandler : IQueryHandler<GetStatesInputQuery, CollectionOutputQuery<StateOutputQuery>>
    {
        private readonly IStateRepository _stateRepository;
        private readonly IMapper _mapper;

        public GetStatesQueryHandler(IStateRepository stateRepository, IMapper mapper)
        {
            _stateRepository = stateRepository;
            _mapper = mapper;
        }

        public async Task<CollectionOutputQuery<StateOutputQuery>> HandleAsync(GetStatesInputQuery inputQuery, CancellationToken cancellationToken = default)
        {
            List<State> states;
            long totalCount;

            if (inputQuery != null)
            {
                states = await _stateRepository.FindAsync(inputQuery.Page, inputQuery.PageSize, inputQuery.Sort, inputQuery.Name, inputQuery.PolishName);
                totalCount = await _stateRepository.CountAsync(inputQuery.Name, inputQuery.PolishName);
            }
            else
            {
                states = await _stateRepository.GetAllAsync();
                totalCount = await _stateRepository.CountAsync();
            }

            var results = _mapper.Map<List<State>, IEnumerable<StateOutputQuery>>(states);
            return new CollectionOutputQuery<StateOutputQuery>(totalCount, results);
        }
    }
}