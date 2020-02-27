using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.Identity.Core.IntegrationEvents;
using Riva.Identity.Core.Services;

namespace Riva.Identity.Core.Commands.Handlers
{
    public class CreateAccountCommandHandler : ICommandHandler<CreateAccountCommand>
    {
        private readonly IAccountVerificationService _accountVerificationService;
        private readonly IAccountCreatorService _accountCreatorService;
        private readonly IIntegrationEventBus _integrationEventBus;

        public CreateAccountCommandHandler(IAccountVerificationService accountVerificationService, IAccountCreatorService accountCreatorService, 
            IIntegrationEventBus integrationEventBus)
        {
            _accountVerificationService = accountVerificationService;
            _accountCreatorService = accountCreatorService;
            _integrationEventBus = integrationEventBus;
        }

        public async Task HandleAsync(CreateAccountCommand command, CancellationToken cancellationToken = default)
        {
            var emailIsNotTakenVerificationResult = await _accountVerificationService.VerifyEmailIsNotTakenAsync(command.Email);
            if(!emailIsNotTakenVerificationResult.Success)
                throw new ConflictException(emailIsNotTakenVerificationResult.Errors);
            
            await _accountCreatorService.CreateAsync(command.AccountId, command.Email, command.Password, command.CorrelationId);

            var accountCreatedIntegrationEvent = new AccountCreatedIntegrationEvent(command.CorrelationId,
                command.AccountId, command.Email, string.Empty);
            await _integrationEventBus.PublishIntegrationEventAsync(accountCreatedIntegrationEvent);
        }
    }
}