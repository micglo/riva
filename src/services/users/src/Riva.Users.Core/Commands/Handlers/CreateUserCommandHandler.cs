using System;
using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.Users.Core.Services;
using Riva.Users.Domain.Users.Aggregates;
using Riva.Users.Domain.Users.Repositories;

namespace Riva.Users.Core.Commands.Handlers
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
    {
        private readonly IUserVerificationService _userVerificationService;
        private readonly IAccountVerificationService _accountVerificationService;
        private readonly IMapper _mapper;
        private readonly ICommunicationBus _communicationBus;
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserVerificationService userVerificationService, IAccountVerificationService accountVerificationService,
            IMapper mapper, ICommunicationBus communicationBus, IUserRepository userRepository)
        {
            _userVerificationService = userVerificationService;
            _accountVerificationService = accountVerificationService;
            _mapper = mapper;
            _communicationBus = communicationBus;
            _userRepository = userRepository;
        }

        public async Task HandleAsync(CreateUserCommand command, CancellationToken cancellationToken = default)
        {
            var userDoesNotExistsVerificationResult = await _userVerificationService.VerifyUserDoesNotExistAsync(command.UserId);
            if(!userDoesNotExistsVerificationResult.Success)
                throw new ConflictException(userDoesNotExistsVerificationResult.Errors);

            var accountExistsVerificationResult = await _accountVerificationService.VerifyAccountExistsAsync(command.UserId, command.Email);
            if (!accountExistsVerificationResult.Success)
                throw new ValidationException(accountExistsVerificationResult.Errors);

            var user = _mapper.Map<CreateUserCommand, User>(command);
            user.AddCreatedEvent(Guid.NewGuid());

            await _communicationBus.DispatchDomainEventsAsync(user, cancellationToken);
            await _userRepository.AddAsync(user);
        }
    }
}