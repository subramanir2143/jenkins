using System;
using System.Collections.Generic;
using System.Text;

namespace Cnx.EarningsAndDeductions.Domain.Interfaces
{
    public interface IEDDomainService
    {
        bool CheckIfEDExists(int empId, string paycodeDescription, DateTime payDateFrom, DateTime payDateTo, DateTime coverageDateFrom, DateTime coverageDateTo);

        bool IsValidPayDates(DateTime payDateFrom, DateTime payDateTo);
    }
}
