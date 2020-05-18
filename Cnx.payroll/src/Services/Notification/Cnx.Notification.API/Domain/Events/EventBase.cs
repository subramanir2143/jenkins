using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cnx.Notification.API.Domain.Events
{
    public class EventBase
    {
        public Guid Id { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}
