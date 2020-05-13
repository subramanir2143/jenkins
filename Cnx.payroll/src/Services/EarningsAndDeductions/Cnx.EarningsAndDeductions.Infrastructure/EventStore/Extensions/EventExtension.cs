using Cnx.EarningsAndDeductions.Domain.AggregatesModel.Interfaces;
using Cnx.EarningsAndDeductions.Infrastructure.EventStore.EventData;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Cnx.EarningsAndDeductions.Infrastructure.EventStore.Extensions
{
    public static class EventExtension
    {
        public static EventDocument ToEventDocument(this IEvent @event)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };

            var serializedEvent = JsonConvert.SerializeObject(@event, settings);

            EventDocument eventDocument = JsonConvert.DeserializeObject(serializedEvent, typeof(EventDocument)) as EventDocument;
            eventDocument.EventType = @event.GetType().Name;         
            
            eventDocument.EventData = BsonSerializer.Deserialize<BsonDocument>(serializedEvent);

            return eventDocument;
        }        
    }
}


