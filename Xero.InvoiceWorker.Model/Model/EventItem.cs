using System;
using System.Collections.Generic;
using System.Text;
using Xero.InvoiceWorker.Model.Enum;
using Xero.InvoiceWorker.Model.Model;

namespace Xero.InvoiceWorker.Model
{
    public class EventItem
    {
        public Status Status { get; set; }
        public DateTime DueDateUtc { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public DateTime UpdatedDateUtc { get; set; }
        public IList<EventLineItem> LineItems { get; set; }
    }
}
