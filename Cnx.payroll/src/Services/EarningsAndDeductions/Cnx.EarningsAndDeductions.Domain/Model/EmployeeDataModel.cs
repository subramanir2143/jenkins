using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cnx.EarningsAndDeductions.Domain.Model
{
    public class EmployeeDataModel
    {
        [Key]
        public int EmployeeId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsActive { get; set; }
    }
}
