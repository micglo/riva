using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.SignalR.Services
{
    public interface IHubWrapperService
    {
        Task SendToGroupAsync(string groupName, IIntegrationEvent integrationEvent);
    }
}