using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Domain;

namespace Riva.BuildingBlocks.Core.Stores
{
    public interface IDomainEventStore
    {
        Task<List<IDomainEvent>> FindAllAsync(Guid aggregateId);
        Task StoreAsync(IDomainEvent domainEvent);
        void Store(IDomainEvent domainEvent);
    }
}