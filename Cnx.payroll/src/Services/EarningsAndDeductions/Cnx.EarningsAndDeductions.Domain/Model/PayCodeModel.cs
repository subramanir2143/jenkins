using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cnx.EarningsAndDeductions.Domain.Model
{
    public class PayCodeModel
    {
        [Key]
        public int Id { get; set; }
        public string PayCode { get; set; }

        public string PayCodeDescription { get; set; }

        public bool IsActive { get; set; }

    }
}
