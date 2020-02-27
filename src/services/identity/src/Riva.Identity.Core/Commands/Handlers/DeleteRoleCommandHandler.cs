using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Roles.Repositories;

namespace Riva.Identity.Core.Commands.Handlers
{
    public class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRoleGetterService _roleGetterService;

        public DeleteRoleCommandHandler(IRoleRepository roleRepository, IRoleGetterService roleGetterService)
        {
            _roleRepository = roleRepository;
            _roleGetterService = roleGetterService;
        }

        public async Task HandleAsync(DeleteRoleCommand command, CancellationToken cancellationToken = default)
        {
            var getRoleResult = await _roleGetterService.GetByIdAsync(command.RoleId);
            if(!getRoleResult.Success)
                throw new ResourceNotFoundException(getRoleResult.Errors);

            if (getRoleResult.Value.RowVersion.Except(command.RowVersion).Any())
                throw new PreconditionFailedException();

            await _roleRepository.DeleteAsync(getRoleResult.Value);
        }
    }
}