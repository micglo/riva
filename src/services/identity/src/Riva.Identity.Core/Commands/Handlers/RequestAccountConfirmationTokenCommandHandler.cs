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
    public class RequestAccountConfirmationTokenCommandHandler : ICommandHandler<RequestAccountConfirmationTokenCommand>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountGetterService _accountGetterService;
        private readonly IAccountVerificationService _accountVerificationService;
        private readonly IAccountConfirmationRequestService _accountConfirmationRequestService;
        private readonly ICommunicationBus _communicationBus;

        public RequestAccountConfirmationTokenCommandHandler(IAccountRepository accountRepository, IAccountGetterService accountGetterService, 
            IAccountVerificationService accountVerificationService, IAccountConfirmationRequestService accountConfirmationRequestService, 
            ICommunicationBus communicationBus)
        {
            _accountRepository = accountRepository;
            _accountGetterService = accountGetterService;
            _accountVerificationService = accountVerificationService;
            _accountConfirmationRequestService = accountConfirmationRequestService;
            _communicationBus = communicationBus;
        }

        public async Task HandleAsync(RequestAccountConfirmationTokenCommand command, CancellationToken cancellationToken = default)
        {
            var getAccountResult = await _accountGetterService.GetByEmailAsync(command.Email);
            if (!getAccountResult.Success)
                throw new ValidationException(getAccountResult.Errors);

            var accountIsNotConfirmedVerificationResult = _accountVerificationService.VerifyAccountIsNotConfirmed(getAccountResult.Value.Confirmed);
            if (!accountIsNotConfirmedVerificationResult.Success)
                throw new ValidationException(accountIsNotConfirmedVerificationResult.Errors);

            var correlationId = Guid.NewGuid();
            var token = getAccountResult.Value.GenerateToken(TokenTypeEnumeration.AccountConfirmation, correlationId);
            await _communicationBus.DispatchDomainEventsAsync(getAccountResult.Value, cancellationToken);
            await _accountRepository.UpdateAsync(getAccountResult.Value);
            await _accountConfirmationRequestService.PublishAccountConfirmationRequestedIntegrationEventAsync(getAccountResult.Value.Email, token.Value, correlationId);
        }
    }
}