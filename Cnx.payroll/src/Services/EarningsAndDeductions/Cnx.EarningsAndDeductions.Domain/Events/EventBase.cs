using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cnx.EarningsAndDeductions.Domain.Events
{
    public class EventBase
    {
        [JsonProperty]
        public string EventState { get; protected set; }
        [JsonProperty]
        public string Description { get; protected set; }
        [JsonProperty]
        public string ClientIP { get; protected set; }
        [JsonProperty]
        public DateTimeOffset EventCreated { get; protected set; }
        [JsonProperty]
        public DateTimeOffset EventLastModified { get; protected set; }
        public string AggregateId { get; protected set; }
        public DateTimeOffset TimeStamp { get; set; }        
        public int Version { get; set; }        
        
        public Guid Id
        {
            get
            {
                return new Guid(AggregateId);
            }
            set
            {
                AggregateId = value.ToString();
            }
        }
       
    }
}
