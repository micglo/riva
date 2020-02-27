using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Riva.BuildingBlocks.Domain
{
    public interface IDomainEventRepository
    {
        Task<List<IDomainEvent>> FindAllAsync(Guid aggregateId);
        Task StoreAsync(IDomainEvent domainEvent);
        void Store(IDomainEvent domainEvent);
    }
}