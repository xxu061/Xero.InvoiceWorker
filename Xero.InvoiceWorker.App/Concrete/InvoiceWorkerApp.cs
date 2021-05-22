using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Xero.InvoiceWorker.App.Interface;
using Xero.InvoiceWorker.Service.Interface;

namespace Xero.InvoiceWorker.App
{
    public class InvoiceWorkerApp: IInvoiceWorkerApp
    {
        private IInvoiceWorkerService _service;
        private ILogger _logger;
        private IAppConfiguration _config;
        public InvoiceWorkerApp(IInvoiceWorkerService service, ILogger logger, IAppConfiguration config)
        {
            _service = service;
            _logger = logger;
            _config = config;
        }
        public async Task Subscribe(string endPoint, string invoiceDirectory)
        {
            if (_config.PageSize > _config.MaxPageSize)
                throw new ArgumentException(string.Format("Page size {0} is greater than max size {1}", _config.PageSize, _config.MaxPageSize));
            _logger.LogInformation("Subscribing to event feed at {0}", _config.InvoiceApiEndpoint);
            await _service.Subscribe(endPoint + _config.InvoiceApiEndpoint, invoiceDirectory);
        }
    }
}
