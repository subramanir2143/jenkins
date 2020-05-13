using System;
using System.Collections.Generic;
using System.Text;

namespace Cnx.EarningsAndDeductions.Domain
{
    public class EDConstants
    {
        public static readonly string CreateEventState = "CREATED";

        public static readonly string EventMessage = "{0} event is successful";

        public static readonly string[] ValidFileActions = { "A", "U", "D" };

        public static readonly string UploadStatusMessage = "Total number of successful ({0}) and failed ({1}) records";

        public static readonly string InvalidAction = "Invalid Action.";

        public static readonly string InvalidPayDates = "Invalid pay date from and date to.";

        public static readonly string InvalidAmount = "Invalid amount.";

        public static readonly string InvalidEmployeeId = "Invalid employee id.";

        public static readonly string Duplicate = "Duplicate Record.";
    }
}
