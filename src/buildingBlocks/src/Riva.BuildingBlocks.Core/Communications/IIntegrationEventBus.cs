using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.BuildingBlocks.Core.Communications
{
    public interface IIntegrationEventBus
    {
        Task PublishIntegrationEventAsync(IIntegrationEvent integrationEvent);
    }
}