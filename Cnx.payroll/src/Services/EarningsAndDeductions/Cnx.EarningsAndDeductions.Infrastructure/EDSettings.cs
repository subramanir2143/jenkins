using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace Cnx.EarningsAndDeductions.Infrastructure
{
    public class EDSettings
    {
        public string MongoConnectionString { get; set; }

        public string MongoDatabase { get; set; }
    }
}
