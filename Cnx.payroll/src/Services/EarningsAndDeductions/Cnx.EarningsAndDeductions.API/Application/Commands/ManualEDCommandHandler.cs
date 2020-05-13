using Cnx.EarningsAndDeductions.Domain.AggregateModel.EarningsAndDeductionAggegate;
using Cnx.EarningsAndDeductions.Domain.AggregatesModel.Interfaces;
using Cnx.EarningsAndDeductions.Domain.Interfaces;
using Cnx.EarningsAndDeductions.Infrastructure.Idempotency;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cnx.EarningsAndDeductions.API.Application.Commands
{
    public class ManualEDCommandHandler : IRequestHandler<ManualEDCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly IDbSession _session;
        private readonly IEDDomainService _edDomainService;

        public ManualEDCommandHandler(IMediator mediator, IDbSession session, IEDDomainService edDomainService)
        {
            _mediator = mediator;
            _session = session;
            _edDomainService = edDomainService;
        }

        public async Task<bool> Handle(ManualEDCommand request, CancellationToken cancellationToken)
        {
            var addCommandResult = request.Added?.Count > 0 ? await Add(request.Added, cancellationToken) : 0;
            var updateCommandResult = request.Updated?.Count > 0 ? await Update(request.Updated, cancellationToken) : 0;
            var deleteCommandResult = request.Deleted?.Count > 0 ? await Delete(request.Deleted, cancellationToken) : 0;

            return addCommandResult + updateCommandResult + deleteCommandResult < 1;
        }

        public async Task<int> Add(List<ManualEDCommand.EarningsAndDeductions> earningsAndDeductionsAddedList, CancellationToken cancellationToken)
        {
            int unsuccessfulAddCount = 0;

            foreach (ManualEDCommand.EarningsAndDeductions earningsAndDeduction in earningsAndDeductionsAddedList)
            {
                bool isDuplicate = _edDomainService.CheckIfEDExists(earningsAndDeduction.EmployeeId, earningsAndDeduction.PayCodeDescription, earningsAndDeduction.PayDateFrom, earningsAndDeduction.PayDateTo, earningsAndDeduction.CoverageDateFrom, earningsAndDeduction.CoverageDateTo);
                bool isValidPayDates = _edDomainService.IsValidPayDates(earningsAndDeduction.PayDateFrom, earningsAndDeduction.PayDateTo);
                if (isDuplicate || !isValidPayDates)
                    unsuccessfulAddCount++;

                if(isValidPayDates)
                    await EarningsAndDeductionRecordOperations(earningsAndDeduction, cancellationToken, isDuplicate);
            }

            return unsuccessfulAddCount;
        }

        public async Task<int> Update(List<ManualEDCommand.EarningsAndDeductions> earningsAndDeductionsUpdatedList, CancellationToken cancellationToken)
        {
            int unsuccesfulEditCount = 0;

            foreach (var updatedEarningsAndDeduction in earningsAndDeductionsUpdatedList)
            {
                bool isValidPayDates = _edDomainService.IsValidPayDates(updatedEarningsAndDeduction.PayDateFrom, updatedEarningsAndDeduction.PayDateTo);

                if (_edDomainService.CheckIfEDExists(updatedEarningsAndDeduction.EmployeeId, updatedEarningsAndDeduction.PayCodeDescription, updatedEarningsAndDeduction.PayDateFrom, updatedEarningsAndDeduction.PayDateTo,
                    updatedEarningsAndDeduction.CoverageDateFrom, updatedEarningsAndDeduction.CoverageDateTo))
                {                    
                    await this.EarningsAndDeductionRecordOperations(updatedEarningsAndDeduction, cancellationToken, true);
                    unsuccesfulEditCount++;
                }
                else
                {
                    if (isValidPayDates)
                    {
                        var recordToBeUpdated = await _session.Get<EarningsAndDeduction>(updatedEarningsAndDeduction.Id);

                        recordToBeUpdated.Update(updatedEarningsAndDeduction.Id, updatedEarningsAndDeduction.TransactionId, updatedEarningsAndDeduction.EmployeeId, updatedEarningsAndDeduction.FirstName, updatedEarningsAndDeduction.LastName, updatedEarningsAndDeduction.PayCode, updatedEarningsAndDeduction.PayCodeDescription, updatedEarningsAndDeduction.PayDateFrom,
                            updatedEarningsAndDeduction.PayDateTo, updatedEarningsAndDeduction.CoverageDateFrom, updatedEarningsAndDeduction.CoverageDateTo, updatedEarningsAndDeduction.Amount, updatedEarningsAndDeduction.Remarks);

                        await _session.Commit(cancellationToken);
                    }
                    else
                        unsuccesfulEditCount++;
                }
            }

            return unsuccesfulEditCount;
        }

        private async Task EarningsAndDeductionRecordOperations(ManualEDCommand.EarningsAndDeductions earningsAndDeduction, CancellationToken cancellationToken, bool isDuplicate)
        {
            EarningsAndDeduction record = new EarningsAndDeduction(Guid.NewGuid(), earningsAndDeduction.EmployeeId,
                        earningsAndDeduction.FirstName, earningsAndDeduction.LastName, earningsAndDeduction.PayCode,
                        earningsAndDeduction.PayCodeDescription, earningsAndDeduction.PayDateFrom, earningsAndDeduction.PayDateTo,
                        earningsAndDeduction.CoverageDateFrom, earningsAndDeduction.CoverageDateTo, earningsAndDeduction.Amount,
                        earningsAndDeduction.Remarks, isDuplicate);

            await _session.Add(record, cancellationToken);
            await _session.Commit(cancellationToken);
        }

        public Task<int> Delete(List<ManualEDCommand.EarningsAndDeductions> earningsAndDeductionsDeletedList, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }


    // Use for Idempotency in Command process
    public class ManualEDIdentifiedCommandHandler : IdentifiedCommandHandler<ManualEDCommand, bool>
    {
        public ManualEDIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<ManualEDCommand, bool>> logger)
            : base(mediator, requestManager, logger)
        {
        }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;                // Ignore duplicate requests for creating order.
        }
    }
}
