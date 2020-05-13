using Cnx.EarningsAndDeductions.Domain.AggregateModel.EarningsAndDeductionAggegate;
using Cnx.EarningsAndDeductions.Domain.Model;
using Cnx.EarningsAndDeductions.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cnx.EarningsAndDeductions.Infrastructure.Repository
{
    public class DuplicateEDRepository : IDuplicateEDRepository
    {
        private readonly EDModelContext _eDModelContext;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _eDModelContext;
            }
        }

        public DuplicateEDRepository(EDModelContext context)
        {
            _eDModelContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public DuplicateEDModel Add(DuplicateEDModel duplicateEDModel)
        {
            return _eDModelContext.DuplicateEDModel.Add(duplicateEDModel).Entity;

        }
    }
}
