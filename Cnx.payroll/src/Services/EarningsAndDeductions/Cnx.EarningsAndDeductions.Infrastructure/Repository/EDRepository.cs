using Cnx.EarningsAndDeductions.Domain.AggregateModel.EarningsAndDeductionAggegate;
using Cnx.EarningsAndDeductions.Domain.Model;
using Cnx.EarningsAndDeductions.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cnx.EarningsAndDeductions.Infrastructure.Repository
{
    public class EDRepository : IEDRepository
    {
        private readonly EDModelContext _eDModelContext;
        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _eDModelContext;
            }
        }

        public EDRepository(EDModelContext context)
        {
            _eDModelContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public EDModel Add(EDModel edModel)
        {
            return _eDModelContext.EDModel.Add(edModel).Entity;

        }

        public int Update(EDModel edModel)
        {
            var recordsList = _eDModelContext.EDModel.Where(em => em.Id == edModel.Id);

            if (recordsList != null && recordsList.Any())
            {
                var recordToBeUpdated = recordsList.FirstOrDefault();
                
                if( recordToBeUpdated.EmployeeId == edModel.EmployeeId && recordToBeUpdated.FirstName == edModel.FirstName && recordToBeUpdated.LastName == edModel.LastName)
                {
                    recordToBeUpdated = edModel;
                    return _eDModelContext.SaveChanges();
                }
                
                return 0;   
            }

            return 0;
        }

        public List<EDModel> GetAll()
        {
            return _eDModelContext.EDModel.ToList();
        }

        public List<PayCodeModel> GetPayCodeList()
        {
            return _eDModelContext.PayCodeModel.ToList();
        }

        public List<EmployeeDataModel> GetEmployeeDataModelList()
        {
            return _eDModelContext.EmployeeDataModel.ToList();
        }
    }
}
