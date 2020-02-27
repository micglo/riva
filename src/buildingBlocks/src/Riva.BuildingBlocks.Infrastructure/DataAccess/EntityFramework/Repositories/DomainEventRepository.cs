using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Riva.BuildingBlocks.Domain;
using Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Models;

namespace Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Repositories
{
    public class DomainEventRepository : IDomainEventRepository
    {
        private readonly DbContext _context;
        private readonly DbSet<DomainEventEntity> _dbSet;

        public DomainEventRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<DomainEventEntity>();
        }

        public async Task<List<IDomainEvent>> FindAllAsync(Guid aggregateId)
        {
            var domainEventEntities = await _dbSet.Where(x => x.AggregateId == aggregateId)
                .OrderBy(x => x.CreationDate)
                .ToListAsync();
            return domainEventEntities.Select(ParseDomainEventEntityToDomainEvent).ToList();
        }

        public async Task<IDomainEvent> FindAsync(Guid aggregateId, Guid correlationId, string fullyQualifiedEventTypeName)
        {
            var domainEventEntity = await _dbSet.SingleOrDefaultAsync(x =>
                x.AggregateId == aggregateId && x.CorrelationId == correlationId &&
                x.FullyQualifiedEventTypeName.Equals(fullyQualifiedEventTypeName));
            return domainEventEntity != null ? ParseDomainEventEntityToDomainEvent(domainEventEntity) : null;
        }

        public async Task StoreAsync(IDomainEvent domainEvent)
        {
            var domainEventEntity = new DomainEventEntity
            {
                AggregateId = domainEvent.AggregateId,
                CorrelationId = domainEvent.CorrelationId,
                CreationDate = domainEvent.CreationDate,
                FullyQualifiedEventTypeName = domainEvent.GetType().AssemblyQualifiedName,
                EventData = JsonConvert.SerializeObject(domainEvent)
            };

            _dbSet.Add(domainEventEntity);
            await _context.SaveChangesAsync();
        }

        public void Store(IDomainEvent domainEvent)
        {
            var domainEventEntity = new DomainEventEntity
            {
                AggregateId = domainEvent.AggregateId,
                CorrelationId = domainEvent.CorrelationId,
                CreationDate = domainEvent.CreationDate,
                FullyQualifiedEventTypeName = domainEvent.GetType().AssemblyQualifiedName,
                EventData = JsonConvert.SerializeObject(domainEvent)
            };

            _dbSet.Add(domainEventEntity);
        }

        private static IDomainEvent ParseDomainEventEntityToDomainEvent(DomainEventEntity domainEventEntity)
        {
            var domainEventType = Type.GetType(domainEventEntity.FullyQualifiedEventTypeName);
            var domainEventObject = JsonConvert.DeserializeObject(domainEventEntity.EventData, domainEventType);
            return (IDomainEvent)domainEventObject;
        }
    }
}