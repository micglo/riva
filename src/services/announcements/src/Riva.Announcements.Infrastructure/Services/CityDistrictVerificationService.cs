using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Riva.Announcements.Core.Enumerations;
using Riva.Announcements.Core.ErrorMessages;
using Riva.Announcements.Core.Services;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.Announcements.Infrastructure.Services
{
    public class CityDistrictVerificationService : ICityDistrictVerificationService
    {
        private readonly IRivaAdministrativeDivisionsApiClientService _rivaAdministrativeDivisionsApiClientService;

        public CityDistrictVerificationService(IRivaAdministrativeDivisionsApiClientService rivaAdministrativeDivisionsApiClientService)
        {
            _rivaAdministrativeDivisionsApiClientService = rivaAdministrativeDivisionsApiClientService;
        }

        public async Task<VerificationResult> VerifyCityDistrictsExistAsync(Guid cityId, IEnumerable<Guid> cityDistrictIds)
        {
            var cityDistricts = await _rivaAdministrativeDivisionsApiClientService.GetCityDistrictsAsync(cityId);

            if (cityDistrictIds.Except(cityDistricts.Select(x => x.Id)).Any())
            {
                var errors = new Collection<IError>
                {
                    new Error(CityDistrictErrorCodeEnumeration.NotFound, CityDistrictErrorMessage.NotFound)
                };
                return VerificationResult.Fail(errors);
            }
            return VerificationResult.Ok();
        }
    }
}