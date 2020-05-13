using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cnx.EarningsAndDeductions.Infrastructure.Idempotency
{
    public class ClientRequest
    {
        public ObjectId Id { get; set; }
        public Guid RequestId { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
    }
}
