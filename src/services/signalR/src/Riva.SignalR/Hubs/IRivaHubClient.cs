using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.SignalR.Hubs
{
    public interface IRivaHubClient
    {
        Task ReceivedIntegrationEvent(IIntegrationEvent integrationEvent);
    }
}