using iText.Html2pdf;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xero.InvoiceWorker.Model;
using Xero.InvoiceWorker.Service.Interface;

namespace Xero.InvoiceWorker.Service.Concrete
{
    public class PdfGenerateService : IPdfGenerateService
    {
        private ILogger _logger;
        public PdfGenerateService(ILogger logger)
        {
            _logger = logger;
        }
        public async Task CreatePdfInvoice(string invoiceDirectory, string templatePath, Event feedEvent)
        {

            if (string.IsNullOrEmpty(invoiceDirectory))
                throw new ArgumentNullException("invoiceDirectory");
            if (string.IsNullOrEmpty(templatePath))
                throw new ArgumentNullException("templatePath");
            if (feedEvent == null)
                throw new ArgumentNullException("feedEvent");

            using (FileStream pdfDest = File.Open(invoiceDirectory + feedEvent.ID + ".pdf", FileMode.OpenOrCreate))
            {
                string htmlSource = await File.ReadAllTextAsync(templatePath);
                string.Format(htmlSource, )
                HtmlConverter.ConvertToPdf(htmlSource, pdfDest);
            }

            _logger.LogInformation("Invoice {0} generate at {1}", feedEvent.ID, invoiceDirectory);
        }

        public async Task DeletePdfInvoice(string invoiceDirectory, Event feedEvent)
        {
            await Task.Run(() =>
            {
                var path = invoiceDirectory + feedEvent.ID + ".pdf";
                File.Delete(path);
                _logger.LogInformation("Invoice at {0} deleted", path);
            });
        }

        public async Task UpdatePdfInvoice(string invoiceDirectory, Event feedEvent)
        {
            throw new NotImplementedException();
        }
    }
}
