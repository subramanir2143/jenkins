using Cnx.EarningsAndDeductions.Domain.AggregatesModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cnx.EarningsAndDeductions.Domain.Events
{
    public class EDInvalidFileEntryDomainEvent : EventBase, IEvent
    {
        public string EmployeeId { get; private set; }

        public string PayCode { get; private set; }

        public string PayDateFrom { get; private set; }
               
        public string PayDateTo { get; private set; }
               
        public string CoverageDateFrom { get; private set; }
               
        public string CoverageDateTo { get; private set; }

        public string Amount { get; private set; }

        public string Remarks { get; private set; }

        public string Action { get; private set; }

        public string Result { get; private set; }

        public int RowNumber { get; private set; }

        public EDInvalidFileEntryDomainEvent(Guid id, string employeeId, string payCode, string payDateFrom,
            string payDateTo, string coverageDateFrom, string coverageDateTo, string amount,
            string remarks, string action, string result, int rowNumber)
        {
            EventCreated = DateTime.UtcNow;
            EventLastModified = DateTime.UtcNow;
            EventState = EDConstants.CreateEventState;
            ClientIP = "";
            Description = string.Format(EDConstants.EventMessage,
                                        nameof(EDFileUploadedDomainEvent));
            Id = id;
            AggregateId = Id.ToString();
            EmployeeId = employeeId;
            PayCode = payCode;
            PayDateFrom = payDateFrom;
            PayDateTo = payDateTo;
            CoverageDateFrom = coverageDateFrom;
            CoverageDateTo = coverageDateTo;
            Amount = amount;
            Remarks = remarks;
            Action = action;
            Result = result;
            RowNumber = rowNumber;
        }
    }
}
