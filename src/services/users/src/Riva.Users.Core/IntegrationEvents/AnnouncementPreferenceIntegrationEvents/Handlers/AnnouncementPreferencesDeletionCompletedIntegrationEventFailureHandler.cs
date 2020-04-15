using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.BuildingBlocks.Core.Enumerations;
using Riva.BuildingBlocks.Core.Logger;
using Riva.BuildingBlocks.Core.Stores;
using Riva.Users.Core.IntegrationEvents.UserIntegrationEvents;
using Riva.Users.Domain.Users.Aggregates;
using Riva.Users.Domain.Users.Events;
using Riva.Users.Domain.Users.Repositories;

namespace Riva.Users.Core.IntegrationEvents.AnnouncementPreferenceIntegrationEvents.Handlers
{
    public class AnnouncementPreferencesDeletionCompletedIntegrationEventFailureHandler : IIntegrationEventHandler<AnnouncementPreferencesDeletionCompletedIntegrationEventFailure>
    {
        private readonly ILogger _logger;
        private readonly IDomainEventStore _domainEventStore;
        private readonly IUserRepository _userRepository;
        private readonly IIntegrationEventBus _integrationEventBus;

        public AnnouncementPreferencesDeletionCompletedIntegrationEventFailureHandler(ILogger logger, IDomainEventStore domainEventStore,
            IUserRepository userRepository, IIntegrationEventBus integrationEventBus)
        {
            _logger = logger;
            _domainEventStore = domainEventStore;
            _userRepository = userRepository;
            _integrationEventBus = integrationEventBus;
        }

        public async Task HandleAsync(AnnouncementPreferencesDeletionCompletedIntegrationEventFailure integrationEvent, CancellationToken cancellationToken = default)
        {
            var message = $"Could not finish {nameof(User)} deletion process.";
            _logger.LogIntegrationEventError(ServiceComponentEnumeration.RivaUsers, integrationEvent,
                "userId={userId}, message={message}, reason={reason}, code={code}",
                integrationEvent.UserId, message, integrationEvent.Reason, integrationEvent.Code);

            var userDeletionCompletedIntegrationEventFailure = new UserDeletionCompletedIntegrationEventFailure(
                integrationEvent.CorrelationId, integrationEvent.Code, integrationEvent.Reason, integrationEvent.UserId);
            var userDeletionCompletedIntegrationEventFailureTask = _integrationEventBus.PublishIntegrationEventAsync(userDeletionCompletedIntegrationEventFailure);

            try
            {
                var domainEvents = await _domainEventStore.FindAllAsync(integrationEvent.UserId);
                var userCreatedDomainEvent = (UserCreatedDomainEvent)domainEvents.First(x => x is UserCreatedDomainEvent);
                var user = User.Builder()
                    .SetId(integrationEvent.UserId)
                    .SetEmail(userCreatedDomainEvent.Email)
                    .SetServiceActive(userCreatedDomainEvent.ServiceActive)
                    .SetAnnouncementPreferenceLimit(userCreatedDomainEvent.AnnouncementPreferenceLimit)
                    .SetAnnouncementSendingFrequency(userCreatedDomainEvent.AnnouncementSendingFrequency)
                    .Build();

                await _userRepository.AddAsync(user);
                user.AddEvents(domainEvents);
                user.ApplyEvents();
                await _userRepository.UpdateAsync(user);
            }
            catch (Exception e)
            {
                _logger.LogIntegrationEventError(ServiceComponentEnumeration.RivaUsers, integrationEvent,
                    "userId={userId}, message={message}, stackTrace={stackTrace}", integrationEvent.UserId,
                    e.Message, e.StackTrace);
            }

            await userDeletionCompletedIntegrationEventFailureTask;
        }
    }
}