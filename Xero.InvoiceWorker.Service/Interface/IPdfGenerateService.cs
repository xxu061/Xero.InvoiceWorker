using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xero.InvoiceWorker.Model;

namespace Xero.InvoiceWorker.Service.Interface
{
    public interface IPdfGenerateService
    {
        Task CreatePdfInvoice(string invoiceDirectory, string templatePath, Event feedEvent);
        Task UpdatePdfInvoice(string invoiceDirectory, Event feedEvent);
        Task DeletePdfInvoice(string invoiceDirectory, Event feedEvent);
    }
}
