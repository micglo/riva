using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications.DomainEvents;
using Riva.BuildingBlocks.Core.Stores;
using Riva.Identity.Domain.Accounts.Events;

namespace Riva.Identity.Core.DomainEventHandlers.AccountDomainEventHandlers
{
    public class AccountRoleDeletedDomainEventHandler : IDomainEventHandler<AccountRoleDeletedDomainEvent>
    {
        private readonly IDomainEventStore _domainEventStore;

        public AccountRoleDeletedDomainEventHandler(IDomainEventStore domainEventStore)
        {
            _domainEventStore = domainEventStore;
        }

        public Task HandleAsync(AccountRoleDeletedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            _domainEventStore.Store(domainEvent);
            return Task.CompletedTask;
        }
    }
}