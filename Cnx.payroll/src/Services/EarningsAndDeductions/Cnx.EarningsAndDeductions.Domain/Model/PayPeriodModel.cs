using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cnx.EarningsAndDeductions.Domain.Model
{
    public class PayPeriodModel
    {
        [Key]
        public Guid PayPeriodId { get; set; }

        public DateTime PayDateFrom { get; set; }

        public DateTime PayDateTo { get; set; }

        public bool IsLocked { get; set; }
    }
}
