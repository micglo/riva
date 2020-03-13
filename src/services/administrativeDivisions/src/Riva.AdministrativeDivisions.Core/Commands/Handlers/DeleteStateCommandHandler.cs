using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Riva.AdministrativeDivisions.Core.Services;
using Riva.AdministrativeDivisions.Domain.States.Repositories;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;

namespace Riva.AdministrativeDivisions.Core.Commands.Handlers
{
    public class DeleteStateCommandHandler : ICommandHandler<DeleteStateCommand>
    {
        private readonly IStateGetterService _stateGetterService;
        private readonly IStateRepository _stateRepository;

        public DeleteStateCommandHandler(IStateGetterService stateGetterService, IStateRepository stateRepository)
        {
            _stateGetterService = stateGetterService;
            _stateRepository = stateRepository;
        }

        public async Task HandleAsync(DeleteStateCommand command, CancellationToken cancellationToken = default)
        {
            var getStateResult = await _stateGetterService.GetByIdAsync(command.StateId);
            if (!getStateResult.Success)
                throw new ResourceNotFoundException(getStateResult.Errors);


            if (getStateResult.Value.RowVersion.Except(command.RowVersion).Any())
                throw new PreconditionFailedException();

            await _stateRepository.DeleteAsync(getStateResult.Value);
        }
    }
}