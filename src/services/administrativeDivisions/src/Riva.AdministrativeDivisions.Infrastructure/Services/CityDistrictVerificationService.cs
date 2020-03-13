using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Riva.AdministrativeDivisions.Core.Enumerations;
using Riva.AdministrativeDivisions.Core.ErrorMessages;
using Riva.AdministrativeDivisions.Core.Services;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Repositories;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.AdministrativeDivisions.Infrastructure.Services
{
    public class CityDistrictVerificationService : ICityDistrictVerificationService
    {
        private readonly ICityDistrictRepository _cityDistrictRepository;
        private readonly ICityDistrictGetterService _cityDistrictGetterService;

        public CityDistrictVerificationService(ICityDistrictRepository cityDistrictRepository, ICityDistrictGetterService cityDistrictGetterService)
        {
            _cityDistrictRepository = cityDistrictRepository;
            _cityDistrictGetterService = cityDistrictGetterService;
        }

        public async Task<VerificationResult> VerifyParentExistsAsync(Guid parentId)
        {
            var getCityDistrictResult = await _cityDistrictGetterService.GetByIdAsync(parentId);
            if (getCityDistrictResult.Success) 
                return VerificationResult.Ok();

            var errors = new Collection<IError>
            {
                new Error(CityDistrictErrorCodeEnumeration.ParentNotFound, CityDistrictErrorMessage.ParentNotFound)
            };
            return VerificationResult.Fail(errors);
        }

        public async Task<VerificationResult> VerifyNameIsNotTakenAsync(string name, Guid cityId)
        {
            return await IsNameAlreadyUsedAsync(name, cityId)
                ? VerificationResult.Fail(new Collection<IError>
                {
                    new Error(CityDistrictErrorCodeEnumeration.NameAlreadyInUse, CityDistrictErrorMessage.NameAlreadyInUse)
                })
                : VerificationResult.Ok();
        }

        public async Task<VerificationResult> VerifyPolishNameIsNotTakenAsync(string polishName, Guid cityId)
        {
            return await IsPolishNameAlreadyUsedAsync(polishName, cityId)
                ? VerificationResult.Fail(new Collection<IError>
                {
                    new Error(CityDistrictErrorCodeEnumeration.PolishNameAlreadyInUse, CityDistrictErrorMessage.PolishNameAlreadyInUse)
                })
                : VerificationResult.Ok();
        }

        private async Task<bool> IsNameAlreadyUsedAsync(string name, Guid cityId)
        {
            return await _cityDistrictRepository.GetByNameAndCityIdAsync(name, cityId) != null;
        }

        private async Task<bool> IsPolishNameAlreadyUsedAsync(string polishName, Guid cityId)
        {
            return await _cityDistrictRepository.GetByPolishNameAndCityIdAsync(polishName, cityId) != null;
        }
    }
}