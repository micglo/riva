using System.Threading;
using System.Threading.Tasks;
using Riva.AdministrativeDivisions.Core.Services;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Aggregates;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.AdministrativeDivisions.Core.Queries.Handlers
{
    public class GetCityDistrictQueryHandler : IQueryHandler<GetCityDistrictInputQuery, CityDistrictOutputQuery>
    {
        private readonly ICityDistrictGetterService _cityDistrictGetterService;
        private readonly IMapper _mapper;

        public GetCityDistrictQueryHandler(ICityDistrictGetterService cityDistrictGetterService, IMapper mapper)
        {
            _cityDistrictGetterService = cityDistrictGetterService;
            _mapper = mapper;
        }

        public async Task<CityDistrictOutputQuery> HandleAsync(GetCityDistrictInputQuery inputQuery, CancellationToken cancellationToken = default)
        {
            var getCityDistrictResult = await _cityDistrictGetterService.GetByIdAsync(inputQuery.CityDistrictId);
            if(!getCityDistrictResult.Success)
                throw new ResourceNotFoundException(getCityDistrictResult.Errors);
            return _mapper.Map<CityDistrict, CityDistrictOutputQuery>(getCityDistrictResult.Value);
        }
    }
}