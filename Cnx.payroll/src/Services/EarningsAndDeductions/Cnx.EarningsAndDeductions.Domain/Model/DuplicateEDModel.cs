using Cnx.EarningsAndDeductions.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cnx.EarningsAndDeductions.Domain.Model
{
    public class DuplicateEDModel : IAggregateRoot
    {
        [Key]
        public long Id { get; set; }

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
    }
}
