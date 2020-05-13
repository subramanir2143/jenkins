using Cnx.EarningsAndDeductions.Domain.Interfaces;
using Cnx.EarningsAndDeductions.Domain.Model;
using Cnx.EarningsAndDeductions.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cnx.EarningsAndDeductions.API.Services
{
    public class EDDomainService : IEDDomainService
    {
        private readonly EDModelContext _eDModelContext;

        public EDDomainService(EDModelContext eDModelContext)
        {
            _eDModelContext = eDModelContext;
            _eDModelContext.Database.EnsureCreated();
        }

        public bool CheckIfEDExists(int empId, string paycodeDescription, DateTime payDateFrom, DateTime payDateTo, DateTime coverageDateFrom, DateTime coverageDateTo)
        {
            bool isExists = this._eDModelContext.EDModel.Any(x => x.EmployeeId == empId
                            && x.PayCodeDescription == paycodeDescription && x.PayDateFrom == payDateFrom
                            && x.PayDateTo == payDateTo && x.CoverageDateFrom == coverageDateFrom && x.CoverageDateTo == coverageDateTo);

            return isExists;
        }

        public bool IsValidPayDates(DateTime payDateFrom, DateTime payDateTo)
        {
            PayPeriodModel model = this._eDModelContext.PayPeriodModel.FirstOrDefault(x => (x.PayDateFrom == payDateFrom) && (x.PayDateTo == payDateTo) && !x.IsLocked);

            return model != null;
        }
    }
}
