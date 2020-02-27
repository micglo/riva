using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.Identity.Core.IntegrationEvents;
using Riva.Identity.Core.Services;

namespace Riva.Identity.Core.Commands.Handlers
{
    public class DeleteAccountCommandHandler : ICommandHandler<DeleteAccountCommand>
    {
        private readonly IAccountGetterService _accountGetterService;
        private readonly ICommunicationBus _communicationBus;
        private readonly IIntegrationEventBus _integrationEventBus;
        private readonly IAccountDataConsistencyService _accountDataConsistencyService;

        public DeleteAccountCommandHandler(IAccountGetterService accountGetterService, ICommunicationBus communicationBus, 
            IIntegrationEventBus integrationEventBus, IAccountDataConsistencyService accountDataConsistencyService)
        {
            _accountGetterService = accountGetterService;
            _communicationBus = communicationBus;
            _integrationEventBus = integrationEventBus;
            _accountDataConsistencyService = accountDataConsistencyService;
        }

        public async Task HandleAsync(DeleteAccountCommand command, CancellationToken cancellationToken = default)
        {
            var getAccountResult = await _accountGetterService.GetByIdAsync(command.AccountId);
            if (!getAccountResult.Success)
                throw new ResourceNotFoundException(getAccountResult.Errors);

            getAccountResult.Value.AddDeletedEvent(command.CorrelationId);
            await _communicationBus.DispatchDomainEventsAsync(getAccountResult.Value, cancellationToken);
            await _accountDataConsistencyService.DeleteAccountWithRelatedPersistedGrants(getAccountResult.Value);

            var accountDeletedIntegrationEvent = new AccountDeletedIntegrationEvent(command.CorrelationId, command.AccountId);
            await _integrationEventBus.PublishIntegrationEventAsync(accountDeletedIntegrationEvent);
        }
    }
}