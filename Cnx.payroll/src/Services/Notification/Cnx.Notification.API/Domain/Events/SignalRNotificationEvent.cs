using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cnx.Notification.API.Domain.Events
{
    public class SignalRNotificationEvent
    {
        public List<string> ConnectionIds { get; set; }
        public string MethodToCall { get; set; }
        public string Arguement1 { get; set; }
        public string Arguement2 { get; set; }
    }
}
