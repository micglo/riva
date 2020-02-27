using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications.DomainEvents;
using Riva.BuildingBlocks.Core.Stores;
using Riva.Identity.Domain.Accounts.Events;

namespace Riva.Identity.Core.DomainEventHandlers.AccountDomainEventHandlers
{
    public class AccountRoleAddedDomainEventHandler : IDomainEventHandler<AccountRoleAddedDomainEvent>
    {
        private readonly IDomainEventStore _domainEventStore;

        public AccountRoleAddedDomainEventHandler(IDomainEventStore domainEventStore)
        {
            _domainEventStore = domainEventStore;
        }

        public Task HandleAsync(AccountRoleAddedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            _domainEventStore.Store(domainEvent);
            return Task.CompletedTask;
        }
    }
}