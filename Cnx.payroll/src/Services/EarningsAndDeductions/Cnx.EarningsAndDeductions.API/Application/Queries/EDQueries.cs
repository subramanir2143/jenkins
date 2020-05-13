using Cnx.EarningsAndDeductions.Domain.AggregateModel.EarningsAndDeductionAggegate;
using Cnx.EarningsAndDeductions.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cnx.EarningsAndDeductions.API.Application.Queries
{
    public class EDQueries : IEDQueries
    {
        private readonly IEDRepository _repository;

        public EDQueries(IEDRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public List<EDViewModel> ReadEDViewModelList()
        {
            List<EDModel> modelList = _repository.GetAll().ToList();

            return modelList.Select(this.PopulateEDViewModel).ToList();
        }

        public List<EmployeeViewModel> ReadEmployeeViewModelList()
        {
            List<EmployeeDataModel> modelList = _repository.GetEmployeeDataModelList().Where(x => x.IsActive).ToList();

            return modelList.Select(this.PopulateEmployeeViewModel).ToList();
        }

        public List<PayCodeViewModel> ReadPayCodeViewModelList()
        {
            List<PayCodeModel> modelList = _repository.GetPayCodeList().Where(x => x.IsActive).ToList();

            return modelList.Select(this.PopulatePayCodeViewModel).ToList();
        }

        private EDViewModel PopulateEDViewModel(EDModel model)
        {
            return new EDViewModel()
            {
                Id = model.Id,
                Amount = model.Amount,
                CoverageDateFrom = model.CoverageDateFrom,
                CoverageDateTo = model.CoverageDateTo,
                EmployeeId = model.EmployeeId,
                FirstName = model.FirstName,
                IsPosted = model.IsPosted,
                IsSubmitted = model.IsSubmitted,
                LastActivity = model.LastActivity,
                LastName = model.LastName,
                PayCode = model.PayCode,
                PayCodeDescription = model.PayCodeDescription,
                PayDateFrom = model.PayDateFrom,
                PayDateTo = model.PayDateTo,
                Remarks = model.Remarks,
                Status = model.Status,
                Terminated = model.Terminated,
                TransactionId = model.TransactionId
            };
        }

        private EmployeeViewModel PopulateEmployeeViewModel(EmployeeDataModel model)
        {
            return new EmployeeViewModel()
            {
                EmployeeId = model.EmployeeId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                IsActive = model.IsActive
            };
        }

        private PayCodeViewModel PopulatePayCodeViewModel(PayCodeModel model)
        {
            return new PayCodeViewModel()
            {
                Id = model.Id,
                PayCode = model.PayCode,
                PayCodeDescription = model.PayCodeDescription,
                IsActive = model.IsActive
            };
        }
    }
}
