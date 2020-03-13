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
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.AdministrativeDivisions.Core.Commands.Handlers
{
    public class CreateCityDistrictCommandHandler : ICommandHandler<CreateCityDistrictCommand>
    {
        private readonly ICityGetterService _cityGetterService;
        private readonly ICityDistrictVerificationService _cityDistrictVerificationService;
        private readonly ICityDistrictRepository _cityDistrictRepository;
        private readonly IMapper _mapper;

        public CreateCityDistrictCommandHandler(ICityGetterService cityGetterService, ICityDistrictVerificationService cityDistrictVerificationService, 
            ICityDistrictRepository cityDistrictRepository, IMapper mapper)
        {
            _cityGetterService = cityGetterService;
            _cityDistrictVerificationService = cityDistrictVerificationService;
            _cityDistrictRepository = cityDistrictRepository;
            _mapper = mapper;
        }

        public async Task HandleAsync(CreateCityDistrictCommand command, CancellationToken cancellationToken = default)
        {
            await CheckCityExistenceAsync(command.CityId);
            await CheckParentExistenceAsync(command.ParentId);
            await CheckDuplicateNamesAsync(command.Name, command.PolishName, command.CityId);
            var city = _mapper.Map<CreateCityDistrictCommand, CityDistrict>(command);
            await _cityDistrictRepository.AddAsync(city);
        }

        private async Task CheckCityExistenceAsync(Guid cityId)
        {
            var getStateResult = await _cityGetterService.GetByIdAsync(cityId);
            if (!getStateResult.Success)
                throw new ValidationException(getStateResult.Errors);
        }

        private async Task CheckParentExistenceAsync(Guid? parentId)
        {
            if (parentId.HasValue)
            {
                var verificationResult = await _cityDistrictVerificationService.VerifyParentExistsAsync(parentId.Value);
                if (!verificationResult.Success)
                    throw new ValidationException(verificationResult.Errors);
            }
        }

        public async Task CheckDuplicateNamesAsync(string name, string polishName, Guid cityId)
        {
            var errors = new List<IError>();

            var verificationResult = await _cityDistrictVerificationService.VerifyNameIsNotTakenAsync(name, cityId);
            if (!verificationResult.Success)
                errors.AddRange(verificationResult.Errors);

            verificationResult = await _cityDistrictVerificationService.VerifyPolishNameIsNotTakenAsync(polishName, cityId);
            if (!verificationResult.Success)
                errors.AddRange(verificationResult.Errors);

            if (errors.Any())
                throw new ConflictException(errors);
        }
    }
}