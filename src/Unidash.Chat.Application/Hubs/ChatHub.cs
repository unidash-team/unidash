using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Unidash.Chat.Application.DataModels;

namespace Unidash.Chat.Application.Hubs
{
    public class ChatHub : Hub<IChatHubClient>
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
