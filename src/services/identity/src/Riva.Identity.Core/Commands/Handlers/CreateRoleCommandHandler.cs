using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Roles.Aggregates;
using Riva.Identity.Domain.Roles.Repositories;

namespace Riva.Identity.Core.Commands.Handlers
{
    public class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRoleVerificationService _roleVerificationService;
        private readonly IMapper _mapper;

        public CreateRoleCommandHandler(IRoleRepository roleRepository, IRoleVerificationService roleVerificationService, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _roleVerificationService = roleVerificationService;
            _mapper = mapper;
        }

        public async Task HandleAsync(CreateRoleCommand command, CancellationToken cancellationToken = default)
        {
            var nameIsNotTakenVerificationResult = await _roleVerificationService.VerifyNameIsNotTakenAsync(command.Name);
            if(!nameIsNotTakenVerificationResult.Success)
                throw new ConflictException(nameIsNotTakenVerificationResult.Errors);

            var role = _mapper.Map<CreateRoleCommand, Role>(command);
            await _roleRepository.AddAsync(role);
        }
    }
}