using System;

namespace Riva.BuildingBlocks.Core.Communications.IntegrationEvents
{
    public abstract class IntegrationEventBase : IIntegrationEvent
    {
        public Guid CorrelationId { get; }
        public DateTimeOffset CreationDate { get; }

        protected IntegrationEventBase()
        {
            CorrelationId = Guid.NewGuid();
            CreationDate = DateTimeOffset.UtcNow;
        }

        protected IntegrationEventBase(Guid correlationId)
        {
            CorrelationId = correlationId;
            CreationDate = DateTimeOffset.UtcNow;
        }

        protected IntegrationEventBase(Guid correlationId, DateTimeOffset creationDate)
        {
            CorrelationId = correlationId;
            CreationDate = creationDate;
        }
    }
}