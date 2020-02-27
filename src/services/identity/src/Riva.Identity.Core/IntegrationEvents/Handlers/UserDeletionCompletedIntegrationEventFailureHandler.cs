using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.BuildingBlocks.Core.Enumerations;
using Riva.BuildingBlocks.Core.Logger;
using Riva.BuildingBlocks.Core.Stores;
using Riva.Identity.Domain.Accounts.Aggregates;
using Riva.Identity.Domain.Accounts.Events;
using Riva.Identity.Domain.Accounts.Repositories;

namespace Riva.Identity.Core.IntegrationEvents.Handlers
{
    public class UserDeletionCompletedIntegrationEventFailureHandler : IIntegrationEventHandler<UserDeletionCompletedIntegrationEventFailure>
    {
        private readonly ILogger _logger;
        private readonly IIntegrationEventBus _integrationEventBus;
        private readonly IDomainEventStore _domainEventStore;
        private readonly IAccountRepository _accountRepository;

        public UserDeletionCompletedIntegrationEventFailureHandler(ILogger logger, IIntegrationEventBus integrationEventBus, 
            IDomainEventStore domainEventStore, IAccountRepository accountRepository)
        {
            _logger = logger;
            _integrationEventBus = integrationEventBus;
            _domainEventStore = domainEventStore;
            _accountRepository = accountRepository;
        }

        public async Task HandleAsync(UserDeletionCompletedIntegrationEventFailure integrationEvent, CancellationToken cancellationToken)
        {
            var message = $"Could not finish {nameof(Account)} deletion process.";
            _logger.LogIntegrationEventError(ServiceComponentEnumeration.RivaIdentity, integrationEvent,
                "accountId={accountId}, message={message}, reason={reason}, code={code}",
                integrationEvent.UserId, message, integrationEvent.Reason, integrationEvent.Code);

            var accountDeletionCompletedIntegrationEventFailure = new AccountDeletionCompletedIntegrationEventFailure(
                integrationEvent.CorrelationId, integrationEvent.Code, integrationEvent.Reason, integrationEvent.UserId);
            var accountDeletionCompletedIntegrationEventFailureTask = _integrationEventBus.PublishIntegrationEventAsync(accountDeletionCompletedIntegrationEventFailure);

            try
            {
                var domainEvents = await _domainEventStore.FindAllAsync(integrationEvent.UserId);
                var accountCreatedDomainEvent = (AccountCreatedDomainEvent)domainEvents.First(x => x is AccountCreatedDomainEvent);
                var account = Account.Builder()
                    .SetId(accountCreatedDomainEvent.AggregateId)
                    .SetEmail(accountCreatedDomainEvent.Email)
                    .SetConfirmed(accountCreatedDomainEvent.Confirmed)
                    .SetPasswordHash(accountCreatedDomainEvent.PasswordHash)
                    .SetSecurityStamp(accountCreatedDomainEvent.SecurityStamp)
                    .SetCreated(accountCreatedDomainEvent.Created)
                    .SetLastLogin(accountCreatedDomainEvent.LastLogin)
                    .Build();

                await _accountRepository.AddAsync(account);
                account.AddEvents(domainEvents);
                account.ApplyEvents();
                await _accountRepository.UpdateAsync(account);
            }
            catch (Exception e)
            {
                _logger.LogIntegrationEventError(ServiceComponentEnumeration.RivaIdentity, integrationEvent,
                    "userId={userId}, message={message}, stackTrace={stackTrace}", integrationEvent.UserId,
                    e.Message, e.StackTrace);
            }

            await accountDeletionCompletedIntegrationEventFailureTask;
        }
    }
}