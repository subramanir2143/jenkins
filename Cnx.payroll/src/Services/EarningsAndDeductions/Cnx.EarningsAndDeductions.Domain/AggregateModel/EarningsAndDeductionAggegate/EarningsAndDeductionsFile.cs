using Cnx.EarningsAndDeductions.Domain.AggregatesModel.Base;
using Cnx.EarningsAndDeductions.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cnx.EarningsAndDeductions.Domain.AggregateModel.EarningsAndDeductionAggegate
{
    public class EarningsAndDeductionsFile : AggregateRoot
    {
        public string FileName { get; private set; }

        public string Uploader { get; private set; }

        public string Result { get; private set; }

        public bool HasErrors { get; private set; }

        public bool IsProcessed { get; protected set; }

        public EarningsAndDeductionsFile()
        {

        }

        public EarningsAndDeductionsFile(Guid id, string fileName, string uploader)
        {
            Id = id;
            FileName = fileName;
            Uploader = uploader;
            ApplyChange(new EDFileUploadedDomainEvent(id, fileName, uploader));
        }

        public void FileProcessed(string result)
        {
            IsProcessed = true;
            //string result = string.Format(EDConstants.UploadStatusMessage, SuccededRecordCount, FailedRecordCount);
            ApplyChange(new EDFileProcessedDomainEvent(Id, result));
        }

        public void InvalidRecord(string employeeId, string payCode, string payDateFrom,
            string payDateTo, string coverageDateFrom, string coverageDateTo, string amount,
            string remarks, string action, string result, int rowNumber)
        {
            HasErrors = true;
            ApplyChange(new EDInvalidFileEntryDomainEvent(Id, employeeId, payCode, payDateFrom,
            payDateTo, coverageDateFrom, coverageDateTo, amount,
            remarks, action, result, rowNumber));
        }

        public void Apply(EDFileUploadedDomainEvent e)
        {
            FileName = FileName;
            Uploader = Uploader;
        }

        public void Apply(EDFileProcessedDomainEvent e)
        {
            Result = Result;
            IsProcessed = IsProcessed;
        }

        public void Apply(EDInvalidFileEntryDomainEvent e)
        {
            HasErrors = HasErrors;
        }
    }
}
