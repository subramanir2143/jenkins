using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cnx.EarningsAndDeductions.API.Application.Queries
{
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsActive { get; set; }
    }
}
