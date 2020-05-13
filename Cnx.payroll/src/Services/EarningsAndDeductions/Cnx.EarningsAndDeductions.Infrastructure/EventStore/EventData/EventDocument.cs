using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cnx.EarningsAndDeductions.Infrastructure.EventStore.EventData
{
    public class EventDocument 
    {
        [BsonId]
        public Guid ObjectId { get; set; }

        [BsonElement("eventId")]
        public Guid Id { get; set; }

        [BsonElement("aggregateId")]
        public string AggregateId { get; set; }

        [BsonElement("eventState")]
        public string EventState { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("clientIp")]
        public string ClientIP { get; set; }

        [BsonElement("eventType")]
        public string EventType { get; set; }

        [BsonElement("version")]
        public int Version { get; set; }

        [BsonElement("eventDateTimeCreated")]
        public DateTimeOffset EventCreated { get; set; }

        [BsonElement("eventDateTimeLastModified")]
        public DateTimeOffset EventLastModified { get; set; }       

        [BsonElement("eventPayload")]
        public BsonDocument EventData { get; set; }        
    }
}
