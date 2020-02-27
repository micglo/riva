using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Roles.Repositories;

namespace Riva.Identity.Core.Commands.Handlers
{
    public class UpdateRoleCommandHandler : ICommandHandler<UpdateRoleCommand>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRoleGetterService _roleGetterService;
        private readonly IRoleVerificationService _roleVerificationService;

        public UpdateRoleCommandHandler(IRoleRepository roleRepository, IRoleGetterService roleGetterService, 
            IRoleVerificationService roleVerificationService)
        {
            _roleRepository = roleRepository;
            _roleGetterService = roleGetterService;
            _roleVerificationService = roleVerificationService;
        }

        public async Task HandleAsync(UpdateRoleCommand command, CancellationToken cancellationToken = default)
        {
            var getRoleResult = await _roleGetterService.GetByIdAsync(command.RoleId);
            if(!getRoleResult.Success)
                throw new ResourceNotFoundException(getRoleResult.Errors);

            if (getRoleResult.Value.RowVersion.Except(command.RowVersion).Any())
                throw new PreconditionFailedException();

            if (!getRoleResult.Value.Name.Equals(command.Name))
            {
                var nameIsNotTakenVerificationResult = await _roleVerificationService.VerifyNameIsNotTakenAsync(command.Name);
                if(!nameIsNotTakenVerificationResult.Success)
                    throw new ConflictException(nameIsNotTakenVerificationResult.Errors);
                getRoleResult.Value.ChangeName(command.Name);
            }
            
            await _roleRepository.UpdateAsync(getRoleResult.Value);
        }
    }
}