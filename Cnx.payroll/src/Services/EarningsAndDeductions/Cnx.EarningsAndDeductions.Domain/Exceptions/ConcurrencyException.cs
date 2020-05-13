using System;
using System.Collections.Generic;
using System.Text;

namespace Cnx.EarningsAndDeductions.Domain.Exceptions
{
    public class ConcurrencyException : System.Exception
    {
        public ConcurrencyException(Guid id)
            : base($"A different version than expected was found in aggregate {id}")
        { }
    }
}
