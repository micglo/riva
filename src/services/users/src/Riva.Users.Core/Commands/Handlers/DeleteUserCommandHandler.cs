using System;
using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.Users.Core.Services;
using Riva.Users.Domain.Users.Repositories;

namespace Riva.Users.Core.Commands.Handlers
{
    public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
    {
        private readonly IUserGetterService _userGetterService;
        private readonly ICommunicationBus _communicationBus;
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IUserGetterService userGetterService, ICommunicationBus communicationBus, 
            IUserRepository userRepository)
        {
            _userGetterService = userGetterService;
            _communicationBus = communicationBus;
            _userRepository = userRepository;
        }

        public async Task HandleAsync(DeleteUserCommand command, CancellationToken cancellationToken = default)
        {
            var getUserResult = await _userGetterService.GetByIdAsync(command.UserId);
            if(!getUserResult.Success)
                throw new ResourceNotFoundException(getUserResult.Errors);

            getUserResult.Value.AddDeletedEvent(Guid.NewGuid());
            await _communicationBus.DispatchDomainEventsAsync(getUserResult.Value, cancellationToken);
            await _userRepository.DeleteAsync(getUserResult.Value);
        }
    }
}