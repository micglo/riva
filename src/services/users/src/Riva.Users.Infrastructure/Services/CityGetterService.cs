using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Models;
using Riva.Users.Core.Enumerations;
using Riva.Users.Core.ErrorMessages;
using Riva.Users.Core.Services;
using Riva.Users.Domain.Cities.Aggregates;
using Riva.Users.Domain.Cities.Repositories;

namespace Riva.Users.Infrastructure.Services
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