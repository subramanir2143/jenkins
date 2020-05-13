using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cnx.EarningsAndDeductions.API.Application.Queries
{
    public interface IEDQueries
    {
        List<EDViewModel> ReadEDViewModelList();

        List<EmployeeViewModel> ReadEmployeeViewModelList();

        List<PayCodeViewModel> ReadPayCodeViewModelList();
    }
}
