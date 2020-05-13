using Cnx.EarningsAndDeductions.Domain.Model;
using Cnx.EarningsAndDeductions.Domain.SeedWork;
using System.Collections.Generic;

namespace Cnx.EarningsAndDeductions.Domain.AggregateModel.EarningsAndDeductionAggegate
{
    public interface IEDRepository : IRepository<EDModel>
    {
        List<EDModel> GetAll();

        EDModel Add(EDModel edModel);

        int Update(EDModel edModel);

        List<PayCodeModel> GetPayCodeList();

        List<EmployeeDataModel> GetEmployeeDataModelList();
    }
}