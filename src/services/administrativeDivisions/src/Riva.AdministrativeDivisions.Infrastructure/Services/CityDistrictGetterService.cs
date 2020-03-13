using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Riva.AdministrativeDivisions.Core.Enumerations;
using Riva.AdministrativeDivisions.Core.ErrorMessages;
using Riva.AdministrativeDivisions.Core.Services;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Aggregates;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Repositories;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.AdministrativeDivisions.Infrastructure.Services
{
    public class CityDistrictGetterService : ICityDistrictGetterService
    {
        private readonly ICityDistrictRepository _cityDistrictRepository;

        public CityDistrictGetterService(ICityDistrictRepository cityDistrictRepository)
        {
            _cityDistrictRepository = cityDistrictRepository;
        }

        public async Task<GetResult<CityDistrict>> GetByIdAsync(Guid id)
        {
            var cityDistrict = await _cityDistrictRepository.GetByIdAsync(id);
            if (cityDistrict is null)
            {
                var errors = new Collection<IError>
                {
                    new Error(CityDistrictErrorCodeEnumeration.NotFound, CityDistrictErrorMessage.NotFound)
                };
                return GetResult<CityDistrict>.Fail(errors);
            }

            return GetResult<CityDistrict>.Ok(cityDistrict);
        }
    }
}