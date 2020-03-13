using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Riva.AdministrativeDivisions.Core.Services;
using Riva.AdministrativeDivisions.Domain.States.Aggregates;
using Riva.AdministrativeDivisions.Domain.States.Repositories;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.AdministrativeDivisions.Core.Commands.Handlers
{
    public class CreateStateCommandHandler : ICommandHandler<CreateStateCommand>
    {
        private readonly IStateRepository _stateRepository;
        private readonly IStateVerificationService _stateVerificationService;
        private readonly IMapper _mapper;

        public CreateStateCommandHandler(IStateRepository stateRepository, IStateVerificationService stateVerificationService, IMapper mapper)
        {
            _stateRepository = stateRepository;
            _stateVerificationService = stateVerificationService;
            _mapper = mapper;
        }

        public async Task HandleAsync(CreateStateCommand command, CancellationToken cancellationToken = default)
        {
            await CheckDuplicateNamesAsync(command.Name, command.PolishName);
            var state = _mapper.Map<CreateStateCommand, State>(command);
            await _stateRepository.AddAsync(state);
        }

        public async Task CheckDuplicateNamesAsync(string name, string polishName)
        {
            var errors = new List<IError>();

            var verificationResult = await _stateVerificationService.VerifyNameIsNotTakenAsync(name);
            if (!verificationResult.Success)
                errors.AddRange(verificationResult.Errors);

            verificationResult = await _stateVerificationService.VerifyPolishNameIsNotTakenAsync(polishName);
            if (!verificationResult.Success)
                errors.AddRange(verificationResult.Errors);

            if (errors.Any())
                throw new ConflictException(errors);
        }
    }
}