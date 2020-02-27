using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications.DomainEvents;
using Riva.BuildingBlocks.Core.Stores;
using Riva.Identity.Domain.Accounts.Events;

namespace Riva.Identity.Core.DomainEventHandlers.AccountDomainEventHandlers
{
    public class AccountTokenGeneratedDomainEventHandler : IDomainEventHandler<AccountTokenGeneratedDomainEvent>
    {
        private readonly IDomainEventStore _domainEventStore;

        public AccountTokenGeneratedDomainEventHandler(IDomainEventStore domainEventStore)
        {
            _domainEventStore = domainEventStore;
        }

        public Task HandleAsync(AccountTokenGeneratedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            _domainEventStore.Store(domainEvent);
            return Task.CompletedTask;
        }
    }
}