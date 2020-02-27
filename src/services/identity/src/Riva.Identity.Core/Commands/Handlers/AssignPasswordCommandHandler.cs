using System;
using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Accounts.Repositories;

namespace Riva.Identity.Core.Commands.Handlers
{
    public class AssignPasswordCommandHandler : ICommandHandler<AssignPasswordCommand>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountGetterService _accountGetterService;
        private readonly IAccountVerificationService _accountVerificationService;
        private readonly IPasswordService _passwordService;
        private readonly ICommunicationBus _communicationBus;

        public AssignPasswordCommandHandler(IAccountRepository accountRepository, IAccountGetterService accountGetterService,
            IAccountVerificationService accountVerificationService, IPasswordService passwordService, ICommunicationBus communicationBus)
        {
            _accountRepository = accountRepository;
            _accountGetterService = accountGetterService;
            _passwordService = passwordService;
            _communicationBus = communicationBus;
            _accountVerificationService = accountVerificationService;
        }

        public async Task HandleAsync(AssignPasswordCommand command, CancellationToken cancellationToken = default)
        {
            var getAccountResult = await _accountGetterService.GetByIdAsync(command.AccountId);
            if(!getAccountResult.Success)
                throw new ResourceNotFoundException(getAccountResult.Errors);

            var passwordIsNotSetVerificationResult = _accountVerificationService.VerifyPasswordIsNotSet(getAccountResult.Value.PasswordHash);
            if (!passwordIsNotSetVerificationResult.Success)
                throw new ValidationException(passwordIsNotSetVerificationResult.Errors);

            getAccountResult.Value.ChangePassword(_passwordService.HashPassword(command.Password), Guid.NewGuid());
            await _communicationBus.DispatchDomainEventsAsync(getAccountResult.Value, cancellationToken);
            await _accountRepository.UpdateAsync(getAccountResult.Value);
        }
    }
}