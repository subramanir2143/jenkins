using System;
using System.Collections.Generic;
using System.Text;

namespace Cnx.EarningsAndDeductions.Domain.AggregatesModel.Base
{
    public class AggregateDescriptor
    {
        public AggregateRoot Aggregate { get; set; }
        public int Version { get; set; }
    }
}
