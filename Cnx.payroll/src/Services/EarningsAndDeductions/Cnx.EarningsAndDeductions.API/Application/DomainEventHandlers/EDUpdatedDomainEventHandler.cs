using Cnx.EarningsAndDeductions.Domain.AggregateModel.EarningsAndDeductionAggegate;
using Cnx.EarningsAndDeductions.Domain.Events;
using Cnx.EarningsAndDeductions.Domain.Model;
using Cnx.EarningsAndDeductions.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cnx.EarningsAndDeductions.API.Application.DomainEventHandlers
{
    public class EDUpdatedDomainEventHandler : INotificationHandler<EDUpdatedDomainEvent>
    {
        private readonly IEDRepository _repository;
        public EDUpdatedDomainEventHandler(IEDRepository repository)
        {
            _repository = repository;

        }

        public async Task Handle(EDUpdatedDomainEvent notification, CancellationToken cancellationToken)
        {
            EDModel eDModel = new EDModel
            {
                Id = notification.Id,
                TransactionId = notification.TransactionId,
                EmployeeId = notification.EmployeeId,
                FirstName = notification.FirstName,
                LastName = notification.LastName,
                PayCode = notification.PayCode,
                PayCodeDescription = notification.PayCodeDescription,
                PayDateFrom = notification.PayDateFrom,
                PayDateTo = notification.PayDateTo,
                CoverageDateFrom = notification.CoverageDateFrom,
                CoverageDateTo = notification.CoverageDateTo,
                Amount = notification.Amount,         
                Remarks = notification.Remarks,
                LastActivity = notification.LastActivity.ToString()
            };

            _repository.Update(eDModel);
            await _repository.UnitOfWork
                .SaveEntitiesAsync(cancellationToken);
        }
    }
}
