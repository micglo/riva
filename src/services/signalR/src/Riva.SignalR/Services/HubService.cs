using System;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.SignalR.Constants;

namespace Riva.SignalR.Services
{
    public class HubService : IHubService
    {
        private readonly IHubWrapperService _hubWrapperService;

        public HubService(IHubWrapperService hubWrapperService)
        {
            _hubWrapperService = hubWrapperService;
        }

        public Task SendToUserAsync(Guid userId, IIntegrationEvent integrationEvent)
        {
            return _hubWrapperService.SendToGroupAsync(userId.ToString(), integrationEvent);
        }

        public Task SendToAllUnauthorizedClientsAsync(IIntegrationEvent integrationEvent)
        {
            return _hubWrapperService.SendToGroupAsync(ConstantVariables.UnauthorizedGroupName, integrationEvent);
        }
    }
}