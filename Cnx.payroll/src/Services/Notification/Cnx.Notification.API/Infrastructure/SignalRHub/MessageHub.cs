using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cnx.Notification.API.Infrastructure.SignalRHub
{
    public class MessageHub : Hub, IMessageHub
    {
        public override Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            Clients.Client(connectionId).SendAsync("updateConnectionId", connectionId);
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
