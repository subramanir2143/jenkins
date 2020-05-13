using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Cnx.EarningsAndDeductions.Domain.AggregatesModel.Interfaces
{
    public interface IEventStore
    {
        Task Save(IEnumerable<IEvent> events, CancellationToken cancellationToken = default(CancellationToken));
        Task<IEnumerable<IEvent>> Get(Guid aggregateId, int fromVersion, CancellationToken cancellationToken = default(CancellationToken));
    }
 
}
