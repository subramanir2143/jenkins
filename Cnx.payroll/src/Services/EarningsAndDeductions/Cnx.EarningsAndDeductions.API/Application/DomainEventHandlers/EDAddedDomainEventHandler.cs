using Cnx.EarningsAndDeductions.Domain.AggregateModel.EarningsAndDeductionAggegate;
using Cnx.EarningsAndDeductions.Domain.Events;
using Cnx.EarningsAndDeductions.Domain.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cnx.EarningsAndDeductions.API.Application.DomainEventHandlers
{
    public class EDAddedDomainEventHandler
        : INotificationHandler<EDAddedDomainEvent>
    {
        private readonly IEDRepository _repository;
        public EDAddedDomainEventHandler(IEDRepository repository)
        {
            _repository = repository;

        }
        public async Task Handle(EDAddedDomainEvent notification, CancellationToken cancellationToken)
        {
            EDModel eDModel = new EDModel
            {
                Id = notification.Id,
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
                Remarks = notification.Remarks,
                Status = notification.Status.ToString(),
                LastActivity = notification.LastActivity.ToString()
            };
            _repository.Add(eDModel);
            await _repository.UnitOfWork
                .SaveEntitiesAsync(cancellationToken);
        }
    }
}
