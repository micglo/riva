using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Riva.AdministrativeDivisions.Domain.Cities.Aggregates;
using Riva.AdministrativeDivisions.Domain.Cities.Repositories;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.AdministrativeDivisions.Core.Queries.Handlers
{
    public class GetCitiesQueryHandler : IQueryHandler<GetCitiesInputQuery, CollectionOutputQuery<CityOutputQuery>>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;

        public GetCitiesQueryHandler(ICityRepository cityRepository, IMapper mapper)
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
        }

        public async Task<CollectionOutputQuery<CityOutputQuery>> HandleAsync(GetCitiesInputQuery inputQuery, CancellationToken cancellationToken = default)
        {
            List<City> cities;
            long totalCount;

            if (inputQuery != null)
            {
                cities = await _cityRepository.FindAsync(inputQuery.Page, inputQuery.PageSize, inputQuery.Sort, inputQuery.StateId, inputQuery.Name, inputQuery.PolishName);
                totalCount = await _cityRepository.CountAsync(inputQuery.StateId, inputQuery.Name, inputQuery.PolishName);
            }
            else
            {
                cities = await _cityRepository.GetAllAsync();
                totalCount = await _cityRepository.CountAsync();
            }

            var results = _mapper.Map<List<City>, IEnumerable<CityOutputQuery>>(cities);
            return new CollectionOutputQuery<CityOutputQuery>(totalCount, results);
        }
    }
}