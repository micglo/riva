using System;
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
    public class RequestPasswordResetTokenCommandHandler : ICommandHandler<RequestPasswordResetTokenCommand>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountGetterService _accountGetterService;
        private readonly IAccountVerificationService _accountVerificationService;
        private readonly IPasswordResetTokenRequestService _passwordResetTokenRequestService;
        private readonly ICommunicationBus _communicationBus;

        public RequestPasswordResetTokenCommandHandler(IAccountRepository accountRepository, IAccountGetterService accountGetterService, 
            IAccountVerificationService accountVerificationService, IPasswordResetTokenRequestService passwordResetTokenRequestService, 
            ICommunicationBus communicationBus)
        {
            _accountRepository = accountRepository;
            _accountGetterService = accountGetterService;
            _accountVerificationService = accountVerificationService;
            _passwordResetTokenRequestService = passwordResetTokenRequestService;
            _communicationBus = communicationBus;
        }

        public async Task HandleAsync(RequestPasswordResetTokenCommand command, CancellationToken cancellationToken = default)
        {
            var getAccountResult = await _accountGetterService.GetByEmailAsync(command.Email);
            if (!getAccountResult.Success)
                throw new ValidationException(getAccountResult.Errors);

            var accountIsConfirmedVerificationResult = _accountVerificationService.VerifyAccountIsConfirmed(getAccountResult.Value.Confirmed);
            if (!accountIsConfirmedVerificationResult.Success)
                throw new ValidationException(accountIsConfirmedVerificationResult.Errors);

            var passwordIsSetVerificationResult = _accountVerificationService.VerifyPasswordIsSet(getAccountResult.Value.PasswordHash);
            if (!passwordIsSetVerificationResult.Success)
                throw new ValidationException(passwordIsSetVerificationResult.Errors);

            var correlationId = Guid.NewGuid();
            var token = getAccountResult.Value.GenerateToken(TokenTypeEnumeration.PasswordReset, correlationId);
            await _communicationBus.DispatchDomainEventsAsync(getAccountResult.Value, cancellationToken);
            await _accountRepository.UpdateAsync(getAccountResult.Value);
            await _passwordResetTokenRequestService.PublishPasswordResetRequestedIntegrationEventAsync(getAccountResult.Value.Email, token.Value, correlationId);
        }
    }
}