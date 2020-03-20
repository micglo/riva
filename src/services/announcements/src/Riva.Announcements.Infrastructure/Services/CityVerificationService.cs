using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Riva.Announcements.Core.Enumerations;
using Riva.Announcements.Core.ErrorMessages;
using Riva.Announcements.Core.Services;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.Announcements.Infrastructure.Services
{
    public class CityVerificationService : ICityVerificationService
    {
        private readonly IRivaAdministrativeDivisionsApiClientService _rivaAdministrativeDivisionsApiClientService;

        public CityVerificationService(IRivaAdministrativeDivisionsApiClientService rivaAdministrativeDivisionsApiClientService)
        {
            _rivaAdministrativeDivisionsApiClientService = rivaAdministrativeDivisionsApiClientService;
        }

        public async Task<VerificationResult> VerifyCityExistsAsync(Guid cityId)
        {
            var city = await _rivaAdministrativeDivisionsApiClientService.GetCityAsync(cityId);
            if (city is null)
            {
                var errors = new Collection<IError>
                {
                    new Error(CityErrorCodeEnumeration.NotFound, CityErrorMessage.NotFound)
                };
                return VerificationResult.Fail(errors);
            }
            return VerificationResult.Ok();
        }
    }
}