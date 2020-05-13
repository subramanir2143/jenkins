using System;
using System.Collections.Generic;
using System.Text;

namespace Cnx.EarningsAndDeductions.Domain.Exceptions
{
    public class EventsOutOfOrderException : System.Exception
    {
        public EventsOutOfOrderException(Guid id)
            : base($"Eventstore gave event for aggregate {id} out of order")
        { }
    }
}
