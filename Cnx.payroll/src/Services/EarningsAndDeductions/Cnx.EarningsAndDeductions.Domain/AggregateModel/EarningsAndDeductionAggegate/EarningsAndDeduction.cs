using Cnx.EarningsAndDeductions.Domain.AggregatesModel.Base;
using Cnx.EarningsAndDeductions.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cnx.EarningsAndDeductions.Domain.AggregateModel.EarningsAndDeductionAggegate
{
    public class EarningsAndDeduction : AggregateRoot
    {
        public long TransactionId { get; private set; }

        public int EmployeeId { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string PayCode { get; private set; }

        public string PayCodeDescription { get; private set; }

        public DateTime PayDateFrom { get; private set; }

        public DateTime PayDateTo { get; private set; }

        public DateTime CoverageDateFrom { get; private set; }

        public DateTime CoverageDateTo { get; private set; }

        public decimal Amount { get; private set; }

        public string Remarks { get; private set; }

        public EDStatus Status { get; private set; }

        public LastActivity LastActivity { get; private set; }

        public bool IsTerminated { get; private set; }

        public bool IsSubmitted { get; private set; }

        public int FileId { get; private set; }

        public EarningsAndDeduction()
        {

        }

        public EarningsAndDeduction(Guid id, int employeeId, string firstName, string lastName, string payCode, string payCodeDescription,
            DateTime payDateFrom, DateTime payDateTo, DateTime coverageDateFrom, DateTime coverageDateTo, decimal amount,
            string remarks, bool isDuplicate)
        {
            Id = id;
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

            if (isDuplicate)
            {
                ApplyChange(new EDDuplicateDomainEvent(id, employeeId, firstName, lastName, payCode, payCodeDescription,
                payDateFrom, payDateTo, coverageDateFrom, coverageDateTo, amount, remarks));
            }
            else
            {
                ApplyChange(new EDAddedDomainEvent(id, employeeId, firstName, lastName, payCode, payCodeDescription,
                payDateFrom, payDateTo, coverageDateFrom, coverageDateTo, amount, remarks));
            }            
        }

        public void Update(Guid id, long transactionId, int employeeId, string firstName, string lastName, string payCode, string payCodeDescription, DateTime payDateFrom, DateTime payDateTo, DateTime coverageDateFrom, DateTime coverageDateTo,
            decimal amount, string remarks)
        {
            ApplyChange(new EDUpdatedDomainEvent(id, transactionId, employeeId, firstName, lastName, payCode, payCodeDescription, payDateFrom, payDateTo, coverageDateFrom, coverageDateTo, amount, remarks));
        }

        private void Apply(EDAddedDomainEvent e)
        {
            EmployeeId = EmployeeId;
            FirstName = FirstName;
            LastName = LastName;
            PayCode = PayCode;
            PayCodeDescription = PayCodeDescription;
            PayDateFrom = PayDateFrom;
            PayDateTo = PayDateTo;
            CoverageDateFrom = CoverageDateFrom;
            CoverageDateTo = CoverageDateTo;
            Amount = Amount;
            Remarks = Remarks;
            Status = Status;
            IsSubmitted = IsSubmitted;
            IsTerminated = IsTerminated;
            FileId = FileId;
        }

        private void Apply(EDUpdatedDomainEvent e)
        {
            PayCode = PayCode;
            PayCodeDescription = PayCodeDescription;
            PayDateFrom = PayDateFrom;
            PayDateTo = PayDateTo;
            CoverageDateFrom = CoverageDateFrom;
            CoverageDateTo = CoverageDateTo;
            Amount = Amount;
            Remarks = Remarks;
        }

        private void Apply(EDDuplicateDomainEvent e)
        {
            EmployeeId = EmployeeId;
            FirstName = FirstName;
            LastName = LastName;
            PayCode = PayCode;
            PayCodeDescription = PayCodeDescription;
            PayDateFrom = PayDateFrom;
            PayDateTo = PayDateTo;
            CoverageDateFrom = CoverageDateFrom;
            CoverageDateTo = CoverageDateTo;
            Amount = Amount;
            Remarks = Remarks;
            Status = Status;
            IsSubmitted = IsSubmitted;
            IsTerminated = IsTerminated;
            FileId = FileId;
        }
    }
}
