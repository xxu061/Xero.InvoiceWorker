using iText.Html2pdf;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xero.InvoiceWorker.Model;
using Xero.InvoiceWorker.Model.Model;
using Xero.InvoiceWorker.Service.Interface;

namespace Xero.InvoiceWorker.Service.Concrete
{
    public class PdfGenerateService : IPdfGenerateService
    {
        private ILogger _logger;
        private readonly string _invoiceTemplateName = "/InvoiceTemplate.html";
        private readonly string _lineItemTemplateName = "/LineItemTemplate.html";
        public PdfGenerateService(ILogger logger)
        {
            _logger = logger;
        }
        public async Task CreatePdfInvoice(string invoiceDirectory, string templateRootPath, Event feedEvent)
        {
            try
            {
                if (ValidateArguments(invoiceDirectory, templateRootPath, feedEvent))
                {
                    using (FileStream pdfDest = File.Open(invoiceDirectory + feedEvent.ID + ".pdf", FileMode.OpenOrCreate))
                    {

                        var invoiceHtml = await MapInvoiceModelToTemplate(templateRootPath, feedEvent);
                        HtmlConverter.ConvertToPdf(invoiceHtml, pdfDest);
                    }

                    _logger.LogInformation("Invoice {0} generate at {1}", feedEvent.ID, invoiceDirectory);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task DeletePdfInvoice(string invoiceDirectory, Event feedEvent)
        {
            await Task.Run(() =>
            {
                var path = invoiceDirectory + feedEvent.ID + ".pdf";
                if (File.Exists(path))
                {
                    File.Delete(path);
                    _logger.LogInformation("Invoice at {0} deleted", path);
                }
                else
                {
                    _logger.LogError("Invoice does not exist at {0}", path);
                    throw new FileNotFoundException(string.Format("Invoice does not exist at {0}", path));
                }
            });
        }

        public async Task UpdatePdfInvoice(string invoiceDirectory, string templateRootPath, Event feedEvent)
        {
            try
            {
                if (ValidateArguments(invoiceDirectory, templateRootPath, feedEvent))
                {
                    await DeletePdfInvoice(invoiceDirectory, feedEvent);
                    await CreatePdfInvoice(invoiceDirectory, templateRootPath, feedEvent);
                    _logger.LogInformation("Invoice {0} updated at {1}", feedEvent.ID, invoiceDirectory);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private bool ValidateArguments(string invoiceDirectory, string templateRootPath, Event feedEvent)
        {
            if (string.IsNullOrEmpty(invoiceDirectory))
                throw new ArgumentNullException("invoiceDirectory");
            if (string.IsNullOrEmpty(templateRootPath))
                throw new ArgumentNullException("templatePath");
            if (feedEvent == null)
                throw new ArgumentNullException("feedEvent");

            return true;
        }

        private async Task<string> MapInvoiceModelToTemplate(string templateRootPath, Event model)
        {
            try
            {
                if (model != null)
                {
                    var htmlSource = await File.ReadAllTextAsync(templateRootPath + _invoiceTemplateName);
                    var result = htmlSource.Replace("{Id}", model.ID.ToString())
                                           .Replace("{Type}", model.Type.ToString())
                                           .Replace("{Content.Status}", model.Content.Status.ToString())
                                           .Replace("{Content.DueDateUtc}", model.Content.DueDateUtc.ToString())
                                           .Replace("{Content.CreatedDateUtc}", model.Content.CreatedDateUtc.ToString())
                                           .Replace("{Content.UpdatedDateUtc}", model.Content.UpdatedDateUtc.ToString())
                                           .Replace("{CreatedDateUtc}", model.CreatedDateUtc.ToString())
                                           .Replace("{LineItems}", await MapLineItemModelToTemplate(templateRootPath, model.Content.LineItems));

                    return result;
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError("Invalid invoice data");
                throw ex;
            }
        }

        private async Task<string> MapLineItemModelToTemplate(string templateRootPath, IList<EventLineItem> items)
        {
            var htmlSource = await File.ReadAllTextAsync(templateRootPath + _lineItemTemplateName);
            StringBuilder sb = new StringBuilder();
            foreach (var item in items)
            {
                if (item != null)
                {
                    sb.Append(htmlSource.Replace("{LineItemId}", item.LineItemId.ToString())
                                        .Replace("{Description}", item.Description)
                                        .Replace("{Quantity}", item.Quantity.ToString())
                                        .Replace("{UnitCost}", item.UnitCost.ToString())
                                        .Replace("{LineItemTotalCost}", item.LineItemTotalCost.ToString()));
                }
                else
                {
                    throw new NullReferenceException();
                }
            }

            return sb.ToString();
        }
    }
}
