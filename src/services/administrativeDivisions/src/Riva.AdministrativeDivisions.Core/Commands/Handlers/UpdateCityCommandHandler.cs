using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Riva.AdministrativeDivisions.Core.Services;
using Riva.AdministrativeDivisions.Domain.Cities.Aggregates;
using Riva.AdministrativeDivisions.Domain.Cities.Repositories;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.AdministrativeDivisions.Core.Commands.Handlers
{
    public class UpdateCityCommandHandler : ICommandHandler<UpdateCityCommand>
    {
        private readonly ICityRepository _cityRepository;
        private readonly ICityGetterService _cityGetterService;
        private readonly ICityVerificationService _cityVerificationService;
        private readonly IStateGetterService _stateGetterService;

        public UpdateCityCommandHandler(ICityRepository cityRepository, ICityGetterService cityGetterService, 
            ICityVerificationService cityVerificationService, IStateGetterService stateGetterService)
        {
            _cityRepository = cityRepository;
            _cityGetterService = cityGetterService;
            _cityVerificationService = cityVerificationService;
            _stateGetterService = stateGetterService;
        }

        public async Task HandleAsync(UpdateCityCommand command, CancellationToken cancellationToken = default)
        {
            var getCityResult = await _cityGetterService.GetByIdAsync(command.CityId);
            if(!getCityResult.Success)
                throw new ResourceNotFoundException(getCityResult.Errors);

            if(getCityResult.Value.RowVersion.Except(command.RowVersion).Any())
                throw new PreconditionFailedException();

            await UpdateStateIdAsync(getCityResult.Value, command.StateId);
            await UpdateNamesAsync(getCityResult.Value, command.Name, command.PolishName, command.StateId);

            await _cityRepository.UpdateAsync(getCityResult.Value);
        }

        private async Task UpdateStateIdAsync(City city, Guid stateId)
        {
            if (city.StateId != stateId)
            {
                var getStateResult = await _stateGetterService.GetByIdAsync(stateId);
                if (!getStateResult.Success)
                    throw new ValidationException(getStateResult.Errors);

                city.ChangeStateId(stateId);
            }
        }

        private async Task UpdateNamesAsync(City city, string name, string polishName, Guid stateId)
        {
            var errors = new List<IError>();

            if (!city.Name.Equals(name))
            {
                var verificationResult = await _cityVerificationService.VerifyNameIsNotTakenAsync(name, stateId);
                if (!verificationResult.Success)
                    errors.AddRange(verificationResult.Errors);
                else
                    city.ChangeName(name);
            }

            if (!city.PolishName.Equals(polishName))
            {
                var verificationResult = await _cityVerificationService.VerifyPolishNameIsNotTakenAsync(polishName, stateId);
                if (!verificationResult.Success)
                    errors.AddRange(verificationResult.Errors);
                else
                    city.ChangePolishName(polishName);
            }

            if (errors.Any())
                throw new ConflictException(errors);
        }
    }
}