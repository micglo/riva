using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Aggregates;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Repositories;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.AdministrativeDivisions.Core.Queries.Handlers
{
    public class GetCityDistrictsQueryHandler : IQueryHandler<GetCityDistrictsInputQuery, CollectionOutputQuery<CityDistrictOutputQuery>>
    {
        private readonly ICityDistrictRepository _cityDistrictRepository;
        private readonly IMapper _mapper;

        public GetCityDistrictsQueryHandler(ICityDistrictRepository cityDistrictRepository, IMapper mapper)
        {
            _cityDistrictRepository = cityDistrictRepository;
            _mapper = mapper;
        }

        public async Task<CollectionOutputQuery<CityDistrictOutputQuery>> HandleAsync(GetCityDistrictsInputQuery inputQuery, CancellationToken cancellationToken = default)
        {
            List<CityDistrict> cityDistricts;
            long totalCount;

            if (inputQuery != null)
            {
                cityDistricts = await _cityDistrictRepository.FindAsync(inputQuery.Page, inputQuery.PageSize, inputQuery.Sort,
                    inputQuery.Name, inputQuery.PolishName, inputQuery.CityId, inputQuery.ParentId, inputQuery.CityIds);
                totalCount =
                    await _cityDistrictRepository.CountAsync(inputQuery.Name, inputQuery.PolishName, inputQuery.CityId,
                        inputQuery.ParentId, inputQuery.CityIds);
            }
            else
            {
                cityDistricts = await _cityDistrictRepository.GetAllAsync();
                totalCount = await _cityDistrictRepository.CountAsync();
            }

            var results = _mapper.Map<List<CityDistrict>, IEnumerable<CityDistrictOutputQuery>>(cityDistricts);
            return new CollectionOutputQuery<CityDistrictOutputQuery>(totalCount, results);
        }
    }
}