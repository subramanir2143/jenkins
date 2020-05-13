using Cnx.EarningsAndDeductions.API;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cnx.EarningsAndDeductions.FunctionalTests
{
    public class EarningsAndDeductionsTestStartup : Startup
    {
        public EarningsAndDeductionsTestStartup(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
