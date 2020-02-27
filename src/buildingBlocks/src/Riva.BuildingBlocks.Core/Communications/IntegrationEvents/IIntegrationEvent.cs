using System;

namespace Riva.BuildingBlocks.Core.Communications.IntegrationEvents
{
    public interface IIntegrationEvent
    {
        Guid CorrelationId { get; }
        DateTimeOffset CreationDate { get; }
    }

    public interface IIntegrationEventFailure : IIntegrationEvent
    {
        string Reason { get; }
        string Code { get; }
    }
}