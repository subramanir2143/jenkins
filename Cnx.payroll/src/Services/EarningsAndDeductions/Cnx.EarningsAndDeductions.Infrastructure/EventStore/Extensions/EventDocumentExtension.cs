using Cnx.EarningsAndDeductions.Domain.AggregatesModel.Interfaces;
using Cnx.EarningsAndDeductions.Infrastructure.EventStore.EventData;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cnx.EarningsAndDeductions.Infrastructure.EventStore.Extensions
{
    public static class EventDocumentExtension
    {
        public static IEnumerable<IEvent> ToIEventList(this List<EventDocument> eventDocuments, List<Type> eventTypes)
        {

            foreach (EventDocument eventDocument in eventDocuments)
            {
                yield return eventDocument.ToEvent(eventTypes.FirstOrDefault(x=>x.Name == eventDocument.EventType));
            }

        }

        public static IEvent ToEvent(this EventDocument eventDocument, Type eventType)
        {
            var eventDataJson = eventDocument.EventData.ToJson();
            var eventObject = JsonConvert.DeserializeObject(eventDataJson, eventType);
            return (IEvent)eventObject;
        }
    }
}
