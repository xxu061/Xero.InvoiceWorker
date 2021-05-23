using iText.Kernel.Pdf;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xero.InvoiceWorker.Model;
using Xero.InvoiceWorker.Model.Enum;
using Xero.InvoiceWorker.Service.Interface;

namespace Xero.InvoiceWorker.Service.Concrete
{
    public class InvoiceWorkerService : IInvoiceWorkerService
    {
        private IPdfGenerateService _pdfService;
        private ILogger _logger;
        public InvoiceWorkerService(IPdfGenerateService pdfService, ILogger logger)
        {
            _pdfService = pdfService;
            _logger = logger;
        }

        public async Task<EventFeed> Subscribe(string endpoint)
        {
            if (!string.IsNullOrEmpty(endpoint) && Regex.IsMatch(endpoint, "^(http|https)://"))
            {
                try
                {
                    using HttpClient client = new HttpClient();
                    var response = await client.GetAsync(endpoint);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<EventFeed>(await response.Content.ReadAsStringAsync());
                    }
                    else
                    {
                        throw new HttpRequestException(string.Format("Unresponsive server {0} with response status {1}", endpoint, response.StatusCode.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new ArgumentNullException(string.Format("Invalid or empty endpoint: {0}", endpoint));
            }
        }

        public async Task ProcessEventFeedCollection(EventFeed eventFeed, string invoiceDirectory, string templatePath)
        {
            if(eventFeed != null && eventFeed.Items != null)
            {
                _logger.LogInformation("Processing invoice items count: {0}", eventFeed.Items.Count);
                List<Task> tasks = new List<Task>();
                foreach (var item in eventFeed.Items)
                {
                    tasks.Add(ProcessInvoiceItem(item, invoiceDirectory, templatePath));
                }

                await Task.WhenAll(tasks.ToArray());
            }
        }
        
        private async Task ProcessInvoiceItem(Event eventItem, string invoiceDirectory, string templatePath)
        {
            if(eventItem != null && !string.IsNullOrEmpty(invoiceDirectory) && !string.IsNullOrEmpty(templatePath))
            {
                _logger.LogInformation("Processing invoice item ID: {0}", eventItem.ID);
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
            else
            {
                throw new ArgumentNullException("Invalid arguments when processing invoice item");
            }

        }
    }
}
