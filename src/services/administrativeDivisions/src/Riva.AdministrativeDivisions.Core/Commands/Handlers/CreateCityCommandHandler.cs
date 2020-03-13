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
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.AdministrativeDivisions.Core.Commands.Handlers
{
    public class CreateCityCommandHandler : ICommandHandler<CreateCityCommand>
    {
        private readonly ICityRepository _cityRepository;
        private readonly ICityVerificationService _cityVerificationService;
        private readonly IStateGetterService _stateGetterService;
        private readonly IMapper _mapper;

        public CreateCityCommandHandler(ICityRepository cityRepository, ICityVerificationService cityVerificationService,
            IStateGetterService stateGetterService, IMapper mapper)
        {
            _cityRepository = cityRepository;
            _cityVerificationService = cityVerificationService;
            _stateGetterService = stateGetterService;
            _mapper = mapper;
        }

        public async Task HandleAsync(CreateCityCommand command, CancellationToken cancellationToken = default)
        {
            await CheckStateExistenceAsync(command.StateId);
            await CheckDuplicateNamesAsync(command.Name, command.PolishName, command.StateId);
            var city = _mapper.Map<CreateCityCommand, City>(command);
            await _cityRepository.AddAsync(city);
        }

        private async Task CheckStateExistenceAsync(Guid stateId)
        {
            var getStateResult = await _stateGetterService.GetByIdAsync(stateId);
            if(!getStateResult.Success)
                throw new ValidationException(getStateResult.Errors);
        }

        public async Task CheckDuplicateNamesAsync(string name, string polishName, Guid stateId)
        {
            var errors = new List<IError>();

            var verificationResult = await _cityVerificationService.VerifyNameIsNotTakenAsync(name, stateId);
            if (!verificationResult.Success)
                errors.AddRange(verificationResult.Errors);

            verificationResult = await _cityVerificationService.VerifyPolishNameIsNotTakenAsync(polishName, stateId);
            if (!verificationResult.Success)
                errors.AddRange(verificationResult.Errors);

            if (errors.Any())
                throw new ConflictException(errors);
        }
    }
}