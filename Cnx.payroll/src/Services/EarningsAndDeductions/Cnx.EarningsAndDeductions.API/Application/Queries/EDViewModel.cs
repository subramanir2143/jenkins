using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cnx.EarningsAndDeductions.API.Application.Queries
{
    public class EDViewModel
    {
        public Guid Id { get; set; }

        public long TransactionId { get; set; }

        public int EmployeeId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PayCode { get; set; }

        public string PayCodeDescription { get; set; }

        public DateTime PayDateFrom { get; set; }

        public DateTime PayDateTo { get; set; }

        public DateTime CoverageDateFrom { get; set; }

        public DateTime CoverageDateTo { get; set; }

        public decimal Amount { get; set; }

        public string Remarks { get; set; }

        public string Status { get; set; }

        public char Terminated { get; set; }

        public string LastActivity { get; set; }

        public string IsSubmitted { get; set; }

        public string IsPosted { get; set; }
    }
}
