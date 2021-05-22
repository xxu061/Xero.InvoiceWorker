using iText.Kernel.Pdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xero.InvoiceWorker.Model;
using Xero.InvoiceWorker.Model.Enum;
using Xero.InvoiceWorker.Service.Interface;

namespace Xero.InvoiceWorker.Service.Concrete
{
    public class InvoiceWorkerService : IInvoiceWorkerService
    {
        private IPdfGenerateService _pdfService;
        public InvoiceWorkerService(IPdfGenerateService pdfService)
        {
            _pdfService = pdfService;
        }

        public async Task<EventFeed> Subscribe(string endpoint)
        {
            if (!string.IsNullOrEmpty(endpoint))
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(endpoint);
                    return JsonConvert.DeserializeObject<EventFeed>(await response.Content.ReadAsStringAsync());
                }
            }
            else
            {
                throw 
            }
        }

        public async Task ProcessEventFeedCollection(EventFeed eventFeed, string invoiceDirectory, string templatePath)
        {
            List<Task> tasks = new List<Task>();
            foreach(var item in eventFeed.Items)
            {
                tasks.Add(ProcessInvoiceItem(item, invoiceDirectory, templatePath));
            }

            await Task.WhenAll(tasks.ToArray());
        }
        
        private async Task ProcessInvoiceItem(Event eventItem, string invoiceDirectory, string templatePath)
        {
            switch (eventItem.Type)
            {
                case EventType.INVOICE_CREATED:
                    await _pdfService.CreatePdfInvoice(invoiceDirectory, templatePath, eventItem);
                    break;
                case EventType.INVOICE_UPDATED:
                    await _pdfService.UpdatePdfInvoice(invoiceDirectory, templatePath, eventItem);
                    break;
                case EventType.INVOICE_DELETED:
                    await _pdfService.DeletePdfInvoice(invoiceDirectory, eventItem);
                    break;

            }
        }
    }
}
