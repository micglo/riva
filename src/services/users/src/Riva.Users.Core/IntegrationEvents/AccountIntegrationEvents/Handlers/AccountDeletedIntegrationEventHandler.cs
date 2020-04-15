using System;
using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.BuildingBlocks.Core.Enumerations;
using Riva.BuildingBlocks.Core.ErrorMessages;
using Riva.BuildingBlocks.Core.Logger;
using Riva.Users.Core.IntegrationEvents.UserIntegrationEvents;
using Riva.Users.Domain.Users.Repositories;

namespace Riva.Users.Core.IntegrationEvents.AccountIntegrationEvents.Handlers
{
    public class AccountDeletedIntegrationEventHandler : IIntegrationEventHandler<AccountDeletedIntegrationEvent>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger;
        private readonly IIntegrationEventBus _integrationEventBus;
        private readonly ICommunicationBus _communicationBus;

        public AccountDeletedIntegrationEventHandler(IUserRepository userRepository, ILogger logger, IIntegrationEventBus integrationEventBus, 
            ICommunicationBus communicationBus)
        {
            _userRepository = userRepository;
            _logger = logger;
            _integrationEventBus = integrationEventBus;
            _communicationBus = communicationBus;
        }

        public async Task HandleAsync(AccountDeletedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(integrationEvent.AccountId);
                user.AddDeletedEvent(integrationEvent.CorrelationId);
                await _communicationBus.DispatchDomainEventsAsync(user, cancellationToken);
                await _userRepository.DeleteAsync(user);

                var userDeletedIntegrationEvent = new UserDeletedIntegrationEvent(integrationEvent.CorrelationId, user.Id);
                await _integrationEventBus.PublishIntegrationEventAsync(userDeletedIntegrationEvent);
            }
            catch (Exception e)
            {
                _logger.LogIntegrationEventError(ServiceComponentEnumeration.RivaUsers, integrationEvent,
                    "userId={userId}, message={message}, stackTrace={stackTrace}", integrationEvent.AccountId,
                    e.Message, e.StackTrace);
                var userDeletionCompletedIntegrationEventFailure = new UserDeletionCompletedIntegrationEventFailure(
                    integrationEvent.CorrelationId, IntegrationEventErrorCodeEnumeration.UnexpectedError.DisplayName,
                    IntegrationEventErrorMessage.UnexpectedError, integrationEvent.AccountId);
                await _integrationEventBus.PublishIntegrationEventAsync(userDeletionCompletedIntegrationEventFailure);
            }
        }
    }
}