using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cnx.Notification.API.Domain.Events
{
    public interface IEvent
    {
        Guid Id { get; set; }
        DateTimeOffset TimeStamp { get; set; }
    }
}
