using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Riva.SignalR.Constants;

namespace Riva.SignalR.Hubs
{
    public class RivaHub : Hub<IRivaHubClient>
    {
        public override async Task OnConnectedAsync()
        {
            if (Context.UserIdentifier is null)
                await Groups.AddToGroupAsync(Context.ConnectionId, ConstantVariables.UnauthorizedGroupName);
            else
                await Groups.AddToGroupAsync(Context.ConnectionId, Context.UserIdentifier);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (Context.UserIdentifier is null)
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, ConstantVariables.UnauthorizedGroupName);
            else
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.UserIdentifier);
            await base.OnDisconnectedAsync(exception);
        }
    }
}