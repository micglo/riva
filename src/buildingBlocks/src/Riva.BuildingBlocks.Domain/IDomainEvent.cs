using System;

namespace Riva.BuildingBlocks.Domain
{
    public interface IDomainEvent
    {
        public Guid AggregateId { get; }
        public Guid CorrelationId { get; }
        DateTimeOffset CreationDate { get; }
    }
}