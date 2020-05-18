using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Cnx.Notification.API.Infrastructure.SignalRHub
{
    public interface IMessageHub
    {
        
        IHubCallerClients Clients { get; set; }
      
        HubCallerContext Context { get; set; }
       
        IGroupManager Groups { get; set; }
    }
}