using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.BuildingBlocks.WebApiTest.IntegrationTests
{
    public class IntegrationEventBusStub : IIntegrationEventBus
    {
        public Task PublishIntegrationEventAsync(IIntegrationEvent integrationEvent)
        {
            return Task.CompletedTask;
        }
    }
}