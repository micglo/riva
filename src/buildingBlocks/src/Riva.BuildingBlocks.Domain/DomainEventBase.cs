using System;

namespace Riva.BuildingBlocks.Domain
{
    public abstract class DomainEventBase : IDomainEvent
    {
        public Guid AggregateId { get; }
        public Guid CorrelationId { get; }
        public DateTimeOffset CreationDate { get; }

        protected DomainEventBase(Guid aggregateId, Guid correlationId)
        {
            AggregateId = aggregateId;
            CorrelationId = correlationId;
            CreationDate = DateTimeOffset.UtcNow;
        }
    }
}