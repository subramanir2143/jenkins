using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Cnx.EarningsAndDeductions.API.Application.Commands
{
    [DataContract]
    public class ManualEDCommand : IRequest<bool>
    {
        [DataMember]
        public List<EarningsAndDeductions> Added { get; set; }

        [DataMember]
        public List<EarningsAndDeductions> Updated { get; set; }

        [DataMember]
        public List<EarningsAndDeductions> Deleted { get; set; }

        public Guid Id { get; set; }

        public ManualEDCommand(List<EarningsAndDeductions> added, List<EarningsAndDeductions> updated, 
            List<EarningsAndDeductions> deleted)
        {
            Added = added;
            Updated = updated;
            Deleted = deleted;
        }

        public class EarningsAndDeductions
        {
            public Guid Id { get; set; }

            public int ExpectedVersion { get; set; }

            public long TransactionId { get; set; }

            public int EmployeeId { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string PayCode { get; set; }

            public string PayCodeDescription { get; set; }

            public DateTime PayDateFrom { get; set; }

            public DateTime PayDateTo { get; set; }

            public DateTime CoverageDateFrom { get; set; }

            public DateTime CoverageDateTo { get; set; }

            public decimal Amount { get; set; }

            public string Remarks { get; set; }

            public EDStatus Status { get; set; }

            public string IsTerminated { get; set; }

            public string IsSubmitted { get; set; }
        }
        public enum EDStatus
        {
            Pending,
            Approved
        }
    }
}
