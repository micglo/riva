using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Accounts.Enumerations;
using Riva.Identity.Domain.Accounts.Repositories;

namespace Riva.Identity.Core.Commands.Handlers
{
    public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountGetterService _accountGetterService;
        private readonly IAccountVerificationService _accountVerificationService;
        private readonly IPasswordService _passwordService;
        private readonly ICommunicationBus _communicationBus;

        public ResetPasswordCommandHandler(IAccountRepository accountRepository, IAccountGetterService accountGetterService, 
            IAccountVerificationService accountVerificationService, IPasswordService passwordService, ICommunicationBus communicationBus)
        {
            _accountRepository = accountRepository;
            _accountGetterService = accountGetterService;
            _accountVerificationService = accountVerificationService;
            _passwordService = passwordService;
            _communicationBus = communicationBus;
        }

        public async Task HandleAsync(ResetPasswordCommand command, CancellationToken cancellationToken = default)
        {
            var getAccountResult = await _accountGetterService.GetByEmailAsync(command.Email);
            if (!getAccountResult.Success)
                throw new ValidationException(getAccountResult.Errors);

            var accountIsConfirmedVerificationResult = _accountVerificationService.VerifyAccountIsConfirmed(getAccountResult.Value.Confirmed);
            if(!accountIsConfirmedVerificationResult.Success)
                throw new ValidationException(accountIsConfirmedVerificationResult.Errors);

            var passwordIsSetVerificationResult = _accountVerificationService.VerifyPasswordIsSet(getAccountResult.Value.PasswordHash);
            if (!passwordIsSetVerificationResult.Success)
                throw new ValidationException(passwordIsSetVerificationResult.Errors);

            var passwordResetToken = getAccountResult.Value.Tokens.SingleOrDefault(x => Equals(x.Type, TokenTypeEnumeration.PasswordReset));
            var confirmationCodeVerificationResult = _accountVerificationService.VerifyConfirmationCode(passwordResetToken, command.Code);
            if (!confirmationCodeVerificationResult.Success)
                throw new ValidationException(confirmationCodeVerificationResult.Errors);

            getAccountResult.Value.ChangePassword(_passwordService.HashPassword(command.Password), Guid.NewGuid());
            await _communicationBus.DispatchDomainEventsAsync(getAccountResult.Value, cancellationToken);
            await _accountRepository.UpdateAsync(getAccountResult.Value);
        }
    }
}