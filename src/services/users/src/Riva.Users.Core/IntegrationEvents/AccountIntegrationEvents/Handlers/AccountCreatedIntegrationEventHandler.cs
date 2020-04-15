using System;
using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.BuildingBlocks.Core.Enumerations;
using Riva.BuildingBlocks.Core.ErrorMessages;
using Riva.BuildingBlocks.Core.Logger;
using Riva.Users.Core.IntegrationEvents.UserIntegrationEvents;
using Riva.Users.Domain.Users.Aggregates;
using Riva.Users.Domain.Users.Defaults;
using Riva.Users.Domain.Users.Repositories;

namespace Riva.Users.Core.IntegrationEvents.AccountIntegrationEvents.Handlers
{
    public class AccountCreatedIntegrationEventHandler : IIntegrationEventHandler<AccountCreatedIntegrationEvent>
    {
        private readonly ICommunicationBus _communicationBus;
        private readonly IIntegrationEventBus _integrationEventBus;
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger;

        public AccountCreatedIntegrationEventHandler(ICommunicationBus communicationBus, IIntegrationEventBus integrationEventBus, 
            IUserRepository userRepository, ILogger logger)
        {
            _communicationBus = communicationBus;
            _integrationEventBus = integrationEventBus;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task HandleAsync(AccountCreatedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        {
            try
            {
                var user = User.Builder()
                    .SetId(integrationEvent.AccountId)
                    .SetEmail(integrationEvent.Email)
                    .SetServiceActive(DefaultUserSettings.ServiceActive)
                    .SetAnnouncementPreferenceLimit(DefaultUserSettings.AnnouncementPreferenceLimit)
                    .SetAnnouncementSendingFrequency(DefaultUserSettings.AnnouncementSendingFrequency)
                    .SetPicture(integrationEvent.Picture)
                    .Build();

                user.AddCreatedEvent(integrationEvent.CorrelationId);

                await _communicationBus.DispatchDomainEventsAsync(user, cancellationToken);
                await _userRepository.AddAsync(user);

                var userCreationCompletedIntegrationEvent = new UserCreationCompletedIntegrationEvent(integrationEvent.CorrelationId, user.Id);
                await _integrationEventBus.PublishIntegrationEventAsync(userCreationCompletedIntegrationEvent);
            }
            catch (Exception e)
            {
                _logger.LogIntegrationEventError(ServiceComponentEnumeration.RivaUsers, integrationEvent,
                    "userId={userId}, message={message}, stackTrace={stackTrace}", integrationEvent.AccountId, e.Message, e.StackTrace);
                var userCreationCompletedIntegrationEventFailure = new UserCreationCompletedIntegrationEventFailure(
                    integrationEvent.CorrelationId, IntegrationEventErrorCodeEnumeration.UnexpectedError.DisplayName,
                    IntegrationEventErrorMessage.UnexpectedError, integrationEvent.AccountId);
                await _integrationEventBus.PublishIntegrationEventAsync(userCreationCompletedIntegrationEventFailure);
            }
        }
    }
}