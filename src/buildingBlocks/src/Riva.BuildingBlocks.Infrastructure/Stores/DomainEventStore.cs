using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Stores;
using Riva.BuildingBlocks.Domain;

namespace Riva.BuildingBlocks.Infrastructure.Stores
{
    public class DomainEventStore : IDomainEventStore
    {
        private readonly IDomainEventRepository _domainEventRepository;

        public DomainEventStore(IDomainEventRepository domainEventRepository)
        {
            _domainEventRepository = domainEventRepository;
        }

        public Task<List<IDomainEvent>> FindAllAsync(Guid aggregateId)
        {
            return _domainEventRepository.FindAllAsync(aggregateId);
        }

        public Task StoreAsync(IDomainEvent domainEvent)
        {
            return _domainEventRepository.StoreAsync(domainEvent);
        }

        public void Store(IDomainEvent domainEvent)
        {
            _domainEventRepository.Store(domainEvent);
        }
    }
}