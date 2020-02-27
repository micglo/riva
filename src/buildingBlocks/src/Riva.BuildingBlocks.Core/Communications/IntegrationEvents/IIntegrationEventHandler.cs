using System.Threading;
using System.Threading.Tasks;

namespace Riva.BuildingBlocks.Core.Communications.IntegrationEvents
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> where TIntegrationEvent : IIntegrationEvent
    {
        Task HandleAsync(TIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
    }
}