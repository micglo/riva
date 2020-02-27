using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications.DomainEvents;
using Riva.BuildingBlocks.Core.Stores;
using Riva.Identity.Domain.Accounts.Events;

namespace Riva.Identity.Core.DomainEventHandlers.AccountDomainEventHandlers
{
    public class AccountCreatedDomainEventHandler : IDomainEventHandler<AccountCreatedDomainEvent>
    {
        private readonly IDomainEventStore _domainEventStore;

        public AccountCreatedDomainEventHandler(IDomainEventStore domainEventStore)
        {
            _domainEventStore = domainEventStore;
        }

        public Task HandleAsync(AccountCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            _domainEventStore.Store(domainEvent);
            return Task.CompletedTask;
        }
    }
}