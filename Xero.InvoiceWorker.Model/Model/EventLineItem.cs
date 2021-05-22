using System;
using System.Collections.Generic;
using System.Text;

namespace Xero.InvoiceWorker.Model.Model
{
    public class EventLineItem
    {
        public Guid LineItemId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal LineItemTotalCost { get; set; }
    }
}
