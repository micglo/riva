using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.BuildingBlocks.Core.Enumerations;
using Riva.BuildingBlocks.Core.ErrorMessages;
using Riva.BuildingBlocks.Core.Logger;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Accounts.Enumerations;

namespace Riva.Identity.Core.IntegrationEvents.Handlers
{
    public class UserCreationCompletedIntegrationEventHandler : IIntegrationEventHandler<UserCreationCompletedIntegrationEvent> 
    {
        private readonly IAccountGetterService _accountGetterService;
        private readonly IAccountConfirmationRequestService _accountConfirmationRequestService;
        private readonly IIntegrationEventBus _integrationEventBus;
        private readonly ILogger _logger;

        public UserCreationCompletedIntegrationEventHandler(IAccountGetterService accountGetterService, IAccountConfirmationRequestService accountConfirmationRequestService, 
            IIntegrationEventBus integrationEventBus, ILogger logger)
        {
            _accountGetterService = accountGetterService;
            _accountConfirmationRequestService = accountConfirmationRequestService;
            _integrationEventBus = integrationEventBus;
            _logger = logger;
        }

        public async Task HandleAsync(UserCreationCompletedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            try
            {
                var getAccountResult = await _accountGetterService.GetByIdAsync(integrationEvent.UserId);

                var accountCreationCompletedIntegrationEvent =
                    new AccountCreationCompletedIntegrationEvent(integrationEvent.CorrelationId,
                        getAccountResult.Value.Id);
                var publishIntegrationEvenTask = _integrationEventBus.PublishIntegrationEventAsync(accountCreationCompletedIntegrationEvent);

                var accountConfirmationToken = getAccountResult.Value.Tokens.SingleOrDefault(x => x.Type.Equals(TokenTypeEnumeration.AccountConfirmation));
                if (accountConfirmationToken != null)
                    await _accountConfirmationRequestService.PublishAccountConfirmationRequestedIntegrationEventAsync(
                        getAccountResult.Value.Email, accountConfirmationToken.Value, integrationEvent.CorrelationId);

                await publishIntegrationEvenTask;
            }
            catch (Exception e)
            {
                _logger.LogIntegrationEventError(ServiceComponentEnumeration.RivaIdentity, integrationEvent,
                    "accountId={accountId}, message={message}, stackTrace={stackTrace}", integrationEvent.UserId,
                    e.Message, e.StackTrace);

                var accountCreationCompletedIntegrationEventFailure =
                    new AccountCreationCompletedIntegrationEventFailure(integrationEvent.CorrelationId,
                        IntegrationEventErrorCodeEnumeration.UnexpectedError.DisplayName,
                        IntegrationEventErrorMessage.UnexpectedError, integrationEvent.UserId);
                await _integrationEventBus.PublishIntegrationEventAsync(accountCreationCompletedIntegrationEventFailure);
            }
        }
    }
}