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
    public class EDDuplicateDomainEventHandler : INotificationHandler<EDDuplicateDomainEvent>
    {
        private readonly IDuplicateEDRepository _duplicateEDRepository;

        public EDDuplicateDomainEventHandler(IDuplicateEDRepository duplicateEDRepository)
        {
            _duplicateEDRepository = duplicateEDRepository;
        }

        public async Task Handle(EDDuplicateDomainEvent notification, CancellationToken cancellationToken)
        {
            DuplicateEDModel duplicateEDModel = new DuplicateEDModel
            {
                Amount = notification.Amount,
                CoverageDateFrom = notification.CoverageDateFrom,
                CoverageDateTo = notification.CoverageDateTo,
                EmployeeId = notification.EmployeeId,
                FirstName = notification.FirstName,
                LastName = notification.LastName,
                PayCode = notification.PayCode,
                PayCodeDescription = notification.PayCodeDescription,
                PayDateFrom = notification.PayDateFrom,
                PayDateTo = notification.PayDateTo,
                Remarks = notification.Remarks
            };
            _duplicateEDRepository.Add(duplicateEDModel);

            await _duplicateEDRepository.UnitOfWork
                .SaveEntitiesAsync(cancellationToken);
        }
    }
}
