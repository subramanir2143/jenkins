using Cnx.EarningsAndDeductions.Domain.Model;
using Cnx.EarningsAndDeductions.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cnx.EarningsAndDeductions.Domain.AggregateModel.EarningsAndDeductionAggegate
{
    public interface IDuplicateEDRepository : IRepository<DuplicateEDModel>
    {
        DuplicateEDModel Add(DuplicateEDModel edModel);
    }
}
