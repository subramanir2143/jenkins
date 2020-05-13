using Cnx.EarningsAndDeductions.Domain.AggregatesModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cnx.EarningsAndDeductions.Domain.Events
{
    public class EDFileUploadedDomainEvent: EventBase, IEvent
    {
        public string FileName { get; protected set; }

        public string Uploader { get; protected set; }

        public EDFileUploadedDomainEvent(Guid id, string fileName, string uploader)
        {
            EventCreated = DateTime.UtcNow;
            EventLastModified = DateTime.UtcNow;
            EventState = EDConstants.CreateEventState;
            ClientIP = "";
            Description = string.Format(EDConstants.EventMessage,
                                        nameof(EDFileUploadedDomainEvent));
            Id = id;
            AggregateId = Id.ToString();
            FileName = fileName;
            Uploader = uploader;
        }
    }
}
