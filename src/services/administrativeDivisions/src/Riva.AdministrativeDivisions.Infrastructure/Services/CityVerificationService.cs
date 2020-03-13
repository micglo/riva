using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Riva.AdministrativeDivisions.Core.Enumerations;
using Riva.AdministrativeDivisions.Core.ErrorMessages;
using Riva.AdministrativeDivisions.Core.Services;
using Riva.AdministrativeDivisions.Domain.Cities.Repositories;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.AdministrativeDivisions.Infrastructure.Services
{
    public class CityVerificationService : ICityVerificationService
    {
        private readonly ICityRepository _cityRepository;

        public CityVerificationService(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public async Task<VerificationResult> VerifyNameIsNotTakenAsync(string name, Guid stateId)
        {
            return await IsNameAlreadyUsedAsync(name, stateId)
                ? VerificationResult.Fail(new Collection<IError>
                {
                    new Error(CityErrorCodeEnumeration.NameAlreadyInUse, CityErrorMessage.NameAlreadyInUse)
                })
                : VerificationResult.Ok();
        }

        public async Task<VerificationResult> VerifyPolishNameIsNotTakenAsync(string polishName, Guid stateId)
        {
            return await IsPolishNameAlreadyUsedAsync(polishName, stateId)
                ? VerificationResult.Fail(new Collection<IError>
                {
                    new Error(CityErrorCodeEnumeration.PolishNameAlreadyInUse, CityErrorMessage.PolishNameAlreadyInUse)
                })
                : VerificationResult.Ok();
        }

        private async Task<bool> IsNameAlreadyUsedAsync(string name, Guid stateId)
        {
            return await _cityRepository.GetByNameAndStateIdAsync(name, stateId) != null;
        }

        private async Task<bool> IsPolishNameAlreadyUsedAsync(string polishName, Guid stateId)
        {
            return await _cityRepository.GetByPolishNameAndStateIdAsync(polishName, stateId) != null;
        }
    }
}