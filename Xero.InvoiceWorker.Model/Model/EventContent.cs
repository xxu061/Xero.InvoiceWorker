using System;
using System.Collections.Generic;
using System.Text;
using Xero.InvoiceWorker.Model.Enum;

namespace Xero.InvoiceWorker.Model
{
    public class EventContent
    {
        public Status Status { get; set; }
        public DateTime DueDateUtc { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public DateTime UpdatedDateUtc { get; set; }
    }
}
