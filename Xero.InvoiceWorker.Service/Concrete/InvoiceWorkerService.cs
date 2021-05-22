using iText.Kernel.Pdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xero.InvoiceWorker.Model;
using Xero.InvoiceWorker.Service.Interface;

namespace Xero.InvoiceWorker.Service.Concrete
{
    public class InvoiceWorkerService : IInvoiceWorkerService
    {
        public async Task<EventFeed> Subscribe(string endpoint, string invoiceDirectory)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(endpoint);
                return JsonConvert.DeserializeObject<EventFeed>(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
