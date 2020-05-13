using Cnx.EarningsAndDeductions.Domain.AggregatesModel.Base;
using Cnx.EarningsAndDeductions.Domain.AggregatesModel.Interfaces;
using Cnx.EarningsAndDeductions.Domain.Exceptions;
using Cnx.EarningsAndDeductions.Infrastructure.Exceptions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cnx.EarningsAndDeductions.Infrastructure.Repositories
{
    public class Repository : IRepository
    {
        private readonly IEventStore _eventStore;
        //private readonly IEventPublisher _publisher;

        public Repository(IEventStore eventStore)
        {
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        }

        public async Task Save<T>(T aggregate, int? expectedVersion = null, CancellationToken cancellationToken = default(CancellationToken)) where T : AggregateRoot
        {
            if (expectedVersion != null && (await _eventStore.Get(aggregate.Id, expectedVersion.Value, cancellationToken)).Any())
            {
                throw new ConcurrencyException(aggregate.Id);
            }

            var changes = aggregate.FlushUncommitedChanges();
            await _eventStore.Save(changes, cancellationToken);
        }

        public Task<T> Get<T>(Guid aggregateId, CancellationToken cancellationToken = default(CancellationToken)) where T : AggregateRoot
        {
            return LoadAggregate<T>(aggregateId, cancellationToken);
        }

        private async Task<T> LoadAggregate<T>(Guid id, CancellationToken cancellationToken = default(CancellationToken)) where T : AggregateRoot
        {
            var events = await _eventStore.Get(id, -1, cancellationToken);
            if (!events.Any())
            {
                throw new AggregateNotFoundException(typeof(T), id);
            }

            var aggregate = AggregateFactory.CreateAggregate<T>();
            aggregate.LoadFromHistory(events);
            return aggregate;
        }
    }
}
