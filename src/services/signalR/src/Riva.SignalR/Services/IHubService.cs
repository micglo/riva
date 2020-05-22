using System;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.SignalR.Services
{
    public interface IHubService
    {
        Task SendToUserAsync(Guid userId, IIntegrationEvent integrationEvent);
        Task SendToAllUnauthorizedClientsAsync(IIntegrationEvent integrationEvent);
    }
}