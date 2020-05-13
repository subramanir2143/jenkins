using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cnx.EarningsAndDeductions.API.Application.Queries
{
    public class PayCodeViewModel
    {
        public int Id { get; set; }

        public string PayCode { get; set; }

        public string PayCodeDescription { get; set; }

        public bool IsActive { get; set; }
    }
}
