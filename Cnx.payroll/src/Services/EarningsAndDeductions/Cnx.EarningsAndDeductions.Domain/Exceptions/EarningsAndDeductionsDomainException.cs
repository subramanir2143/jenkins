using System;
using System.Collections.Generic;
using System.Text;

namespace Cnx.EarningsAndDeductions.Domain.Exceptions
{
    public class EarningsAndDeductionsDomainException : Exception
    {
        public EarningsAndDeductionsDomainException()
        { }

        public EarningsAndDeductionsDomainException(string message)
            : base(message)
        { }

        public EarningsAndDeductionsDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
