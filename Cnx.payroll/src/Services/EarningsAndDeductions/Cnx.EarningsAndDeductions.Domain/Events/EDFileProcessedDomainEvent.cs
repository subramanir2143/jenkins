using Cnx.EarningsAndDeductions.Domain.AggregatesModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cnx.EarningsAndDeductions.Domain.Events
{
    public class EDFileProcessedDomainEvent : EventBase, IEvent
    {
        public string Result { get; protected set; }

        public EDFileProcessedDomainEvent(Guid id, string result)
        {
            EventCreated = DateTime.UtcNow;
            EventLastModified = DateTime.UtcNow;
            EventState = EDConstants.CreateEventState;
            ClientIP = "";
            Description = string.Format(EDConstants.EventMessage,
                                        nameof(EDFileProcessedDomainEvent));
            Id = id;
            AggregateId = Id.ToString();
            Result = result;
        }
    }
}
