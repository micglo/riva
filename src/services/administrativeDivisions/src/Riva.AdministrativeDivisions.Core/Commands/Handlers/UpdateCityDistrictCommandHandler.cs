using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Riva.AdministrativeDivisions.Core.Services;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Aggregates;
using Riva.AdministrativeDivisions.Domain.CityDistricts.Repositories;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.AdministrativeDivisions.Core.Commands.Handlers
{
    public class UpdateCityDistrictCommandHandler : ICommandHandler<UpdateCityDistrictCommand>
    {
        private readonly ICityDistrictGetterService _cityDistrictGetterService;
        private readonly ICityGetterService _cityGetterService;
        private readonly ICityDistrictVerificationService _cityDistrictVerificationService;
        private readonly ICityDistrictRepository _cityDistrictRepository;

        public UpdateCityDistrictCommandHandler(ICityDistrictGetterService cityDistrictGetterService, 
            ICityGetterService cityGetterService, ICityDistrictVerificationService cityDistrictVerificationService, 
            ICityDistrictRepository cityDistrictRepository)
        {
            _cityDistrictGetterService = cityDistrictGetterService;
            _cityGetterService = cityGetterService;
            _cityDistrictVerificationService = cityDistrictVerificationService;
            _cityDistrictRepository = cityDistrictRepository;
        }

        public async Task HandleAsync(UpdateCityDistrictCommand command, CancellationToken cancellationToken = default)
        {
            var getCityDistrictResult = await _cityDistrictGetterService.GetByIdAsync(command.CityDistrictId);
            if (!getCityDistrictResult.Success)
                throw new ResourceNotFoundException(getCityDistrictResult.Errors);

            if (getCityDistrictResult.Value.RowVersion.Except(command.RowVersion).Any())
                throw new PreconditionFailedException();

            await UpdateCityIdAsync(getCityDistrictResult.Value, command.CityId);
            await UpdateParentIdAsync(getCityDistrictResult.Value, command.ParentId);
            await UpdateNamesAsync(getCityDistrictResult.Value, command.Name, command.PolishName, command.CityId);
            UpdateNameVariants(getCityDistrictResult.Value, command.NameVariants.ToList());

            await _cityDistrictRepository.UpdateAsync(getCityDistrictResult.Value);
        }

        private async Task UpdateCityIdAsync(CityDistrict cityDistrict, Guid cityId)
        {
            if (cityDistrict.CityId != cityId)
            {
                var getCityResult = await _cityGetterService.GetByIdAsync(cityId);
                if (!getCityResult.Success)
                    throw new ValidationException(getCityResult.Errors);

                cityDistrict.ChangeCityId(cityId);
            }
        }

        private async Task UpdateParentIdAsync(CityDistrict cityDistrict, Guid? parentId)
        {
            if (cityDistrict.ParentId != parentId)
            {
                if (parentId.HasValue)
                {
                    var verificationResult = await _cityDistrictVerificationService.VerifyParentExistsAsync(parentId.Value);
                    if (!verificationResult.Success)
                        throw new ValidationException(verificationResult.Errors);
                }

                cityDistrict.ChangeParentId(parentId);
            }
        }

        private async Task UpdateNamesAsync(CityDistrict cityDistrict, string name, string polishName, Guid cityId)
        {
            var errors = new List<IError>();

            if (!cityDistrict.Name.Equals(name))
            {
                var verificationResult = await _cityDistrictVerificationService.VerifyNameIsNotTakenAsync(name, cityId);
                if (!verificationResult.Success)
                    errors.AddRange(verificationResult.Errors);
                else
                    cityDistrict.ChangeName(name);
            }

            if (!cityDistrict.PolishName.Equals(polishName))
            {
                var verificationResult = await _cityDistrictVerificationService.VerifyPolishNameIsNotTakenAsync(polishName, cityId);
                if (!verificationResult.Success)
                    errors.AddRange(verificationResult.Errors);
                else
                    cityDistrict.ChangePolishName(polishName);
            }

            if (errors.Any())
                throw new ConflictException(errors);
        }

        private static void UpdateNameVariants(CityDistrict cityDistrict, ICollection<string> nameVariants)
        {
            var nameVariantsToRemove = cityDistrict.NameVariants.Except(nameVariants).ToList();
            var nameVariantsToAdd = nameVariants.Except(cityDistrict.NameVariants).ToList();

            foreach (var nameVariant in nameVariantsToRemove)
            {
                cityDistrict.RemoveNameVariant(nameVariant);
            }
            
            foreach (var nameVariant in nameVariantsToAdd)
            {
                cityDistrict.AddNameVariant(nameVariant);
            }
        }
    }
}