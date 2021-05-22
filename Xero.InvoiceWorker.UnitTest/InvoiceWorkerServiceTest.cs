using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xero.InvoiceWorker.App;
using Xero.InvoiceWorker.App.Interface;
using Xero.InvoiceWorker.Service.Concrete;
using Xero.InvoiceWorker.Service.Interface;
using Xunit;

namespace Xero.InvoiceWorker.UnitTest
{
    public class InvoiceWorkerServiceTest
    {
        public InvoiceWorkerServiceTest()
        {

        }

        [Fact]
        public async Task ShouldGetEventFeed()
        {
            Mock<IPdfGenerateService> pdfService = new Mock<IPdfGenerateService>();
            IInvoiceWorkerService service = new InvoiceWorkerService(pdfService.Object);
            var endpoint = "http://someendpoint";
            var result = await service.Subscribe(endpoint);
        }
    }
}
