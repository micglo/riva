using System;

namespace Riva.BuildingBlocks.Infrastructure.DataAccess.EntityFramework.Models
{
    public class DomainEventEntity : EntityBase
    {
        public Guid AggregateId { get; set; }
        public Guid CorrelationId { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public string FullyQualifiedEventTypeName { get; set; }
        public string EventData { get; set; }
    }
}