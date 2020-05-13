using Cnx.EarningsAndDeductions.Domain.AggregatesModel.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cnx.EarningsAndDeductions.Domain.AggregatesModel.Interfaces
{
    public interface IRepository
    {
        Task Save<T>(T aggregate, int? expectedVersion = null, CancellationToken cancellationToken = default(CancellationToken)) where T : AggregateRoot;
        Task<T> Get<T>(Guid aggregateId, CancellationToken cancellationToken = default(CancellationToken)) where T : AggregateRoot;
    }
}


