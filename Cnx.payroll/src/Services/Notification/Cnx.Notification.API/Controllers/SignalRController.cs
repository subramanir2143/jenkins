using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cnx.Notification.API.Infrastructure.SignalRHub;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Cnx.Notification.API.Controllers
{
    [Route("api/v1/[controller]")]
    public class SignalRController : Controller
    {
        private readonly IHubContext<MessageHub> _hubcontext;

        public SignalRController(IHubContext<MessageHub> hubcontext)
        {
            _hubcontext = hubcontext;
        }
        [HttpPost]
        [Route("SendMessage")]
        public async void SendMessage(IReadOnlyList<string> connectionIds, string methodToCall, string arguement1, string arguement2)
        {
            await _hubcontext.Clients.Clients(connectionIds).SendAsync(methodToCall, arguement1, arguement2);
        }

    }
}