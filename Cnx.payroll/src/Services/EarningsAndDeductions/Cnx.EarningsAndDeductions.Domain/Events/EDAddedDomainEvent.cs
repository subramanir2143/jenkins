using Cnx.EarningsAndDeductions.Domain.AggregateModel.EarningsAndDeductionAggegate;
using Cnx.EarningsAndDeductions.Domain.AggregatesModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Cnx.EarningsAndDeductions.Domain.Events
{
    public class EDAddedDomainEvent : EventBase, IEvent, INotification
    {
        public int EmployeeId { get; protected set; }

        public string FirstName { get; protected set; }

        public string LastName { get; protected set; }

        public string PayCode { get; protected set; }

        public string PayCodeDescription { get; protected set; }

        public DateTime PayDateFrom { get; protected set; }

        public DateTime PayDateTo { get; protected set; }

        public DateTime CoverageDateFrom { get; protected set; }

        public DateTime CoverageDateTo { get; protected set; }

        public decimal Amount { get; protected set; }

        public string Remarks { get; protected set; }

        public EDStatus Status { get; protected set; }

        public LastActivity LastActivity { get; protected set; }

        public long TransactionId { get; protected set; }

        public EDAddedDomainEvent(Guid id, int employeeId, string firstName, string lastName, string payCode, string payCodeDescription,
            DateTime payDateFrom, DateTime payDateTo, DateTime coverageDateFrom, DateTime coverageDateTo, decimal amount,
            string remarks)
        {
            EventCreated = DateTime.UtcNow;
            EventLastModified = DateTime.UtcNow;
            EventState = EDConstants.CreateEventState;
            ClientIP = "";
            Description = string.Format(EDConstants.EventMessage,
                                        nameof(EDAddedDomainEvent));
            Id = id;
            AggregateId = Id.ToString();
            EmployeeId = employeeId;
            FirstName = firstName;
            LastName = lastName;
            PayCode = payCode;
            PayCodeDescription = payCodeDescription;
            PayDateFrom = payDateFrom;
            PayDateTo = payDateTo;
            CoverageDateFrom = coverageDateFrom;
            CoverageDateTo = coverageDateTo;
            Amount = amount;
            Remarks = remarks;
            Status = EDStatus.Pending;
        }
    }
}
