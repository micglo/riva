using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Riva.AdministrativeDivisions.Core.Enumerations;
using Riva.AdministrativeDivisions.Core.ErrorMessages;
using Riva.AdministrativeDivisions.Core.Services;
using Riva.AdministrativeDivisions.Domain.Cities.Aggregates;
using Riva.AdministrativeDivisions.Domain.Cities.Repositories;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.AdministrativeDivisions.Infrastructure.Services
{
    public class CityGetterService : ICityGetterService
    {
        private readonly ICityRepository _cityRepository;

        public CityGetterService(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public async Task<GetResult<City>> GetByIdAsync(Guid id)
        {
            var city = await _cityRepository.GetByIdAsync(id);
            if (city is null)
            {
                var errors = new Collection<IError>
                {
                    new Error(CityErrorCodeEnumeration.NotFound, CityErrorMessage.NotFound)
                };
                return GetResult<City>.Fail(errors);
            }

            return GetResult<City>.Ok(city);
        }
    }
}