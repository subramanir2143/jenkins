using Cnx.EarningsAndDeductions.Domain.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cnx.EarningsAndDeductions.API.Application.DomainEventHandlers
{
    public class EDFileProcessedDomainEventHandler
        : INotificationHandler<EDFileProcessedDomainEvent>
    {
        public Task Handle(EDFileProcessedDomainEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
