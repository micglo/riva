using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.SignalR.Hubs;

namespace Riva.SignalR.Services
{
    public class HubWrapperService : IHubWrapperService
    {
        private readonly IHubContext<RivaHub, IRivaHubClient> _context;

        public HubWrapperService(IHubContext<RivaHub, IRivaHubClient> context)
        {
            _context = context;
        }

        public Task SendToGroupAsync(string groupName, IIntegrationEvent integrationEvent)
        {
            return _context.Clients.Group(groupName).ReceivedIntegrationEvent(integrationEvent);
        }
    }
}