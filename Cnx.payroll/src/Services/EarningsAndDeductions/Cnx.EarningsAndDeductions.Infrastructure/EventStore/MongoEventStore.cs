using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Cnx.EarningsAndDeductions.Domain.AggregatesModel.Interfaces;
using Cnx.EarningsAndDeductions.Domain.Events;
using Cnx.EarningsAndDeductions.Infrastructure.EventStore.EventData;
using Cnx.EarningsAndDeductions.Infrastructure.EventStore.Extensions;
using Cnx.EarningsAndDeductions.Infrastructure.Idempotency;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Cnx.EarningsAndDeductions.Infrastructure.EventStore
{
    public class MongoEventStore : IEventStore
    {
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<EventDocument> _collection;
        private readonly List<Type> _eventTypes;
        private readonly IMediator _mediator;

        public MongoEventStore(IOptions<EDSettings> mongoOptions, IMediator mediator)
        {
            _client = new MongoClient(mongoOptions.Value.MongoConnectionString);
            _database = _client.GetDatabase(mongoOptions.Value.MongoDatabase);
            _collection = _database.GetCollection<EventDocument>("events");
            _eventTypes = Assembly.Load(typeof(IEvent).Assembly.FullName)
                          .GetTypes()
                          .Where(t => typeof(IEvent).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                          .ToList();
            _mediator = mediator;
        }
        public IMongoCollection<ClientRequest> ClientRequests
        {
            get
            {
                return _database.GetCollection<ClientRequest>("ClientRequests");
            }
        }



        public async Task Save(IEnumerable<IEvent> events, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var session = _client.StartSession())
            {
                foreach (var @event in events)
                {
                    session.StartTransaction();
                    try
                    {
                        EventDocument eventDocument = @event.ToEventDocument();
                        _collection.InsertOne(eventDocument);
                        
                        await _mediator.Publish((INotification)@event);
                        session.CommitTransaction();
                    }
                    catch
                    {
                        session.AbortTransaction();
                        throw;
                    }
                    //TODO Add a property in event 
                }
            }
        }

        // Don't forget to create the index according to this query
        public async Task<IEnumerable<IEvent>> Get(Guid aggregateId, Int32 fromVersion, CancellationToken cancellationToken = default(CancellationToken))
        {
            var builder = Builders<EventDocument>.Filter;
            var filter = builder.Eq("AggregateId", aggregateId.ToString()) & builder.Gt("Version", fromVersion);

            var list = _collection
                        .Find(filter)
                        .ToList();

            return list.ToIEventList(_eventTypes);
        }
    }
}
