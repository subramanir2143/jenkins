using System;
using System.Collections.Generic;
using System.Text;

namespace Cnx.EarningsAndDeductions.Infrastructure.Exceptions
{
    public class AggregateNotFoundException : Exception
    {
        public AggregateNotFoundException(Type t, Guid id)
            : base($"Aggregate {id} of type {t.FullName} was not found")
        { }
    }
}
