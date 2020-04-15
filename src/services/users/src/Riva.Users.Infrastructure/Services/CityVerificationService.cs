using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Models;
using Riva.Users.Core.Enumerations;
using Riva.Users.Core.ErrorMessages;
using Riva.Users.Core.Services;

namespace Riva.Users.Infrastructure.Services
{
    public class CityVerificationService : ICityVerificationService
    {
        private readonly ICityGetterService _cityGetterService;

        public CityVerificationService(ICityGetterService cityGetterService)
        {
            _cityGetterService = cityGetterService;
        }

        public async Task<VerificationResult> VerifyCityAndCityDistrictsAsync(Guid cityId, IEnumerable<Guid> cityDistrictIds)
        {
            var getCityResult = await _cityGetterService.GetByIdAsync(cityId);
            if(!getCityResult.Success)
                return VerificationResult.Fail(getCityResult.Errors);

            var incorrectCityDistricts = cityDistrictIds.Except(getCityResult.Value.CityDistricts);
            if (incorrectCityDistricts.Any())
            {
                var errors = new Collection<IError>
                {
                    new Error(CityErrorCodeEnumeration.IncorrectCityDistricts, CityErrorMessage.IncorrectCityDistricts)
                };
                return VerificationResult.Fail(errors);
            }
            return VerificationResult.Ok();
        }
    }
}