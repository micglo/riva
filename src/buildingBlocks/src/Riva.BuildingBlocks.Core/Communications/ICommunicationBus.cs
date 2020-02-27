using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.BuildingBlocks.Domain;

namespace Riva.BuildingBlocks.Core.Communications
{
    public interface ICommunicationBus
    {
        Task SendCommandAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : ICommand;

        Task DispatchDomainEventsAsync<TAggregate>(TAggregate aggregate, CancellationToken cancellationToken = default) 
            where TAggregate : AggregateBase;

        Task PublishIntegrationEventAsync<TIntegrationEvent>(TIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
            where TIntegrationEvent : IIntegrationEvent;
    }
}