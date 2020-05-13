using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cnx.EarningsAndDeductions.Domain.AggregatesModel.Interfaces
{
    public interface IEvent : INotification
    {
        Guid Id { get; set; }
        int Version { get; set; }
        DateTimeOffset TimeStamp { get; set; }
    }
}
