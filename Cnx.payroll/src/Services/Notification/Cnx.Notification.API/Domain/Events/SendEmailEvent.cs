using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cnx.Notification.API.Domain.Events
{
    public class SendEmailEvent : EventBase, IEvent
    {
        public List<string> ToAddress { get; set; }
        public List<string> CCAddress { get; set; }
        public List<string> BccAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
       
    }
}
