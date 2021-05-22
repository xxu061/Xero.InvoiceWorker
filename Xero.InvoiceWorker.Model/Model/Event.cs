using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using Xero.InvoiceWorker.Model.Enum;

namespace Xero.InvoiceWorker.Model
{
    public class Event
    {
        public int ID { get; set; }
        public EventType Type { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public EventItem Content { get; set; }
    }
}
