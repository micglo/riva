using System.Threading;
using System.Threading.Tasks;
using Riva.AdministrativeDivisions.Core.Services;
using Riva.AdministrativeDivisions.Domain.Cities.Aggregates;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.AdministrativeDivisions.Core.Queries.Handlers
{
    public class GetCityQueryHandler : IQueryHandler<GetCityInputQuery, CityOutputQuery>
    {
        private readonly ICityGetterService _cityGetterService;
        private readonly IMapper _mapper;

        public GetCityQueryHandler(ICityGetterService cityGetterService, IMapper mapper)
        {
            _cityGetterService = cityGetterService;
            _mapper = mapper;
        }

        public async Task<CityOutputQuery> HandleAsync(GetCityInputQuery inputQuery, CancellationToken cancellationToken = default)
        {
            var getCityResult = await _cityGetterService.GetByIdAsync(inputQuery.CityId);
            if(!getCityResult.Success)
                throw new ResourceNotFoundException(getCityResult.Errors);
            return _mapper.Map<City, CityOutputQuery>(getCityResult.Value);
        }
    }
}