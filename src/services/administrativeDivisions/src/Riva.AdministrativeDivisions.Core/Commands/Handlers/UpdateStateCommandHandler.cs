using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Riva.AdministrativeDivisions.Core.Services;
using Riva.AdministrativeDivisions.Domain.States.Aggregates;
using Riva.AdministrativeDivisions.Domain.States.Repositories;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.AdministrativeDivisions.Core.Commands.Handlers
{
    public class UpdateStateCommandHandler : ICommandHandler<UpdateStateCommand>
    {
        private readonly IStateRepository _stateRepository;
        private readonly IStateGetterService _stateGetterService;
        private readonly IStateVerificationService _stateVerificationService;

        public UpdateStateCommandHandler(IStateRepository stateRepository, IStateGetterService stateGetterService, 
            IStateVerificationService stateVerificationService)
        {
            _stateRepository = stateRepository;
            _stateGetterService = stateGetterService;
            _stateVerificationService = stateVerificationService;
        }

        public async Task HandleAsync(UpdateStateCommand command, CancellationToken cancellationToken = default)
        {
            var getStateResult = await _stateGetterService.GetByIdAsync(command.StateId);
            if (!getStateResult.Success)
                throw new ResourceNotFoundException(getStateResult.Errors);

            if (getStateResult.Value.RowVersion.Except(command.RowVersion).Any())
                throw new PreconditionFailedException();

            await UpdateNamesAsync(getStateResult.Value, command.Name, command.PolishName);

            await _stateRepository.UpdateAsync(getStateResult.Value);
        }

        private async Task UpdateNamesAsync(State state, string name, string polishName)
        {
            var errors = new List<IError>();

            if (!state.Name.Equals(name))
            {
                var verificationResult = await _stateVerificationService.VerifyNameIsNotTakenAsync(name);
                if (!verificationResult.Success)
                    errors.AddRange(verificationResult.Errors);
                else
                    state.ChangeName(name);
            }

            if (!state.PolishName.Equals(polishName))
            {
                var verificationResult = await _stateVerificationService.VerifyPolishNameIsNotTakenAsync(polishName);
                if (!verificationResult.Success)
                    errors.AddRange(verificationResult.Errors);
                else
                    state.ChangePolishName(polishName);
            }

            if (errors.Any())
                throw new ConflictException(errors);
        }
    }
}