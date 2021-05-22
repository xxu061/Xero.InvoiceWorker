using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xero.InvoiceWorker.Model;

namespace Xero.InvoiceWorker.Service.Interface
{
    public interface IInvoiceWorkerService
    {
        Task<EventFeed> Subscribe(string endpoint);
        Task ProcessEventFeedCollection(EventFeed eventFeed, string invoiceDirectory, string templatePath);
    }
}
