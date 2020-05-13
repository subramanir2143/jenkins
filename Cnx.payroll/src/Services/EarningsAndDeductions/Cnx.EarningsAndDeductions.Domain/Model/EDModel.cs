using Cnx.EarningsAndDeductions.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cnx.EarningsAndDeductions.Domain.Model
{
    public class EDModel: IAggregateRoot
    {
        public Guid Id { get; set; }
        public int FileId { get; set; }
        public long Sequence { get; set; }
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
