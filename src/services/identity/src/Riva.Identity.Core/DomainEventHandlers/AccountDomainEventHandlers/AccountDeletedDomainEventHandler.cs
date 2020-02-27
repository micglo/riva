using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications.DomainEvents;
using Riva.BuildingBlocks.Core.Stores;
using Riva.Identity.Domain.Accounts.Events;

namespace Riva.Identity.Core.DomainEventHandlers.AccountDomainEventHandlers
{
    public class AccountDeletedDomainEventHandler : IDomainEventHandler<AccountDeletedDomainEvent>
    {
        private readonly IDomainEventStore _domainEventStore;

        public AccountDeletedDomainEventHandler(IDomainEventStore domainEventStore)
        {
            _domainEventStore = domainEventStore;
        }

        public Task HandleAsync(AccountDeletedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            _domainEventStore.Store(domainEvent);
            return Task.CompletedTask;
        }
    }
}