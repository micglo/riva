using System;
using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.BuildingBlocks.Core.Enumerations;
using Riva.BuildingBlocks.Core.Logger;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Accounts.Aggregates;

namespace Riva.Identity.Core.IntegrationEvents.Handlers
{
    public class UserCreationCompletedIntegrationEventFailureHandler : IIntegrationEventHandler<UserCreationCompletedIntegrationEventFailure>
    {
        private readonly IAccountGetterService _accountGetterService;
        private readonly IAccountDataConsistencyService _accountDataConsistencyService;
        private readonly IIntegrationEventBus _integrationEventBus;
        private readonly ILogger _logger;

        public UserCreationCompletedIntegrationEventFailureHandler(IAccountGetterService accountGetterService, 
            IAccountDataConsistencyService accountDataConsistencyService, IIntegrationEventBus integrationEventBus, 
            ILogger logger)
        {
            _accountGetterService = accountGetterService;
            _integrationEventBus = integrationEventBus;
            _logger = logger;
            _accountDataConsistencyService = accountDataConsistencyService;
        }

        public async Task HandleAsync(UserCreationCompletedIntegrationEventFailure integrationEvent, CancellationToken cancellationToken)
        {
            var message = $"Could not finish {nameof(Account)} creation process.";
            _logger.LogIntegrationEventError(ServiceComponentEnumeration.RivaIdentity, integrationEvent,
                "accountId={accountId}, message={message}, reason={reason}, code={code}",
                integrationEvent.UserId, message, integrationEvent.Reason, integrationEvent.Code);

            var accountCreationCompletedIntegrationEventFailure =
                new AccountCreationCompletedIntegrationEventFailure(integrationEvent.CorrelationId,
                    integrationEvent.Code, integrationEvent.Reason, integrationEvent.UserId);
            var completeAccountCreationProcessWithFailureTask =
                _integrationEventBus.PublishIntegrationEventAsync(accountCreationCompletedIntegrationEventFailure);

            try
            {
                var getAccountResult = await _accountGetterService.GetByIdAsync(integrationEvent.UserId);
                await _accountDataConsistencyService.DeleteAccountWithRelatedPersistedGrants(getAccountResult.Value);
            }
            catch (Exception e)
            {
                _logger.LogIntegrationEventError(ServiceComponentEnumeration.RivaIdentity, integrationEvent,
                    "accountId={accountId}, message={message}, stackTrace={stackTrace}", integrationEvent.UserId,
                    e.Message, e.StackTrace);
            }
            finally
            {
                await completeAccountCreationProcessWithFailureTask;
            }
        }
    }
}