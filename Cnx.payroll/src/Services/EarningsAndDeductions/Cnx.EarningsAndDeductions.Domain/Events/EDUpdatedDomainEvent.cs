using Cnx.EarningsAndDeductions.Domain.AggregateModel.EarningsAndDeductionAggegate;
using Cnx.EarningsAndDeductions.Domain.AggregatesModel.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cnx.EarningsAndDeductions.Domain.Events
{
    public class EDUpdatedDomainEvent : EventBase, IEvent, INotification
    {
        public long TransactionId { get; protected set; }

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

        public LastActivity LastActivity { get; set; }

        public EDUpdatedDomainEvent(Guid id, long transactionId, int employeeId, string firstName, string lastName, string payCode, string payCodeDescription, DateTime payDateFrom, DateTime payDateTo, DateTime coverageDateFrom,
            DateTime coverageDateTo, decimal amount, string remarks)
        {
            Id = id;
            AggregateId = Id.ToString();
            ClientIP = "";
            Description = "EDUpdatedDomainEvent event is successful";
            EventCreated = DateTime.UtcNow;
            EventState = "CREATED";
            EventLastModified = DateTime.UtcNow;
            TransactionId = transactionId;
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
            LastActivity = LastActivity.Edited;
        }
    }
}
