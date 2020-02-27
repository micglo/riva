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
    public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountGetterService _accountGetterService;
        private readonly IAccountVerificationService _accountVerificationService;
        private readonly IPasswordService _passwordService;
        private readonly ICommunicationBus _communicationBus;

        public ChangePasswordCommandHandler(IAccountRepository accountRepository, IAccountGetterService accountGetterService, 
            IAccountVerificationService accountVerificationService, IPasswordService passwordService, ICommunicationBus communicationBus)
        {
            _accountRepository = accountRepository;
            _accountGetterService = accountGetterService;
            _accountVerificationService = accountVerificationService;
            _passwordService = passwordService;
            _communicationBus = communicationBus;
        }

        public async Task HandleAsync(ChangePasswordCommand command, CancellationToken cancellationToken = default)
        {
            var getAccountResult = await _accountGetterService.GetByIdAsync(command.AccountId);
            if (!getAccountResult.Success)
                throw new ResourceNotFoundException(getAccountResult.Errors);

            var passwordIsSetVerificationResult = _accountVerificationService.VerifyPasswordIsSet(getAccountResult.Value.PasswordHash);
            if (!passwordIsSetVerificationResult.Success)
                throw new ValidationException(passwordIsSetVerificationResult.Errors);

            var passwordIsCorrectVerificationResult = _accountVerificationService.VerifyPassword(getAccountResult.Value.PasswordHash, command.OldPassword);
            if (!passwordIsCorrectVerificationResult.Success)
                throw new ValidationException(passwordIsCorrectVerificationResult.Errors);

            getAccountResult.Value.ChangePassword(_passwordService.HashPassword(command.NewPassword), Guid.NewGuid());
            await _communicationBus.DispatchDomainEventsAsync(getAccountResult.Value, cancellationToken);
            await _accountRepository.UpdateAsync(getAccountResult.Value);
        }
    }
}