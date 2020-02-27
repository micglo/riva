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
    public class ConfirmAccountCommandHandler : ICommandHandler<ConfirmAccountCommand>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountGetterService _accountGetterService;
        private readonly IAccountVerificationService _accountVerificationService;
        private readonly ICommunicationBus _communicationBus;

        public ConfirmAccountCommandHandler(IAccountRepository accountRepository, IAccountGetterService accountGetterService, 
            IAccountVerificationService accountVerificationService, ICommunicationBus communicationBus)
        {
            _accountRepository = accountRepository;
            _accountGetterService = accountGetterService;
            _accountVerificationService = accountVerificationService;
            _communicationBus = communicationBus;
        }

        public async Task HandleAsync(ConfirmAccountCommand command, CancellationToken cancellationToken = default)
        {
            var getAccountResult = await _accountGetterService.GetByEmailAsync(command.Email);
            if(!getAccountResult.Success)
                throw new ValidationException(getAccountResult.Errors);

            var accountIsNotConfirmedVerificationResult = _accountVerificationService.VerifyAccountIsNotConfirmed(getAccountResult.Value.Confirmed);
            if(!accountIsNotConfirmedVerificationResult.Success)
                throw new ValidationException(accountIsNotConfirmedVerificationResult.Errors);

            var accountConfirmationToken = getAccountResult.Value.Tokens.SingleOrDefault(x => Equals(x.Type, TokenTypeEnumeration.AccountConfirmation));
            var confirmationCodeVerificationResult = _accountVerificationService.VerifyConfirmationCode(accountConfirmationToken, command.Code);
            if (!confirmationCodeVerificationResult.Success)
                throw new ValidationException(confirmationCodeVerificationResult.Errors);

            getAccountResult.Value.Confirm(Guid.NewGuid());
            await _communicationBus.DispatchDomainEventsAsync(getAccountResult.Value, cancellationToken);
            await _accountRepository.UpdateAsync(getAccountResult.Value);
        }
    }
}