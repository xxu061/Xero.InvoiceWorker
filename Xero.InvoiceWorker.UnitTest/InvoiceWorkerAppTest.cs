using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xero.InvoiceWorker.App;
using Xero.InvoiceWorker.App.Interface;
using Xero.InvoiceWorker.Service.Interface;
using Xunit;

namespace Xero.InvoiceWorker.UnitTest
{
    public class InvoiceWorkerAppTest
    {
        public InvoiceWorkerAppTest()
        {

        }

        [Fact]
        public async Task ShouldGetEventFeed()
        {
            Mock<IInvoiceWorkerService> service = new Mock<IInvoiceWorkerService>();
            Mock<ILogger> logger = new Mock<ILogger>();
            Mock<IAppConfiguration> config = new Mock<IAppConfiguration>();
            IInvoiceWorkerApp mockApp = new InvoiceWorkerApp(service.Object, logger.Object, config.Object);
            var endpoint = "http://someendpoint";
            var invoiceDirectory = "someinvoicedirectorylocation";
            await mockApp.Subscribe(endpoint, invoiceDirectory);
        }
    }
}
