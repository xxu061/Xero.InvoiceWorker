using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xero.InvoiceWorker.App;
using Xero.InvoiceWorker.App.Interface;
using Xero.InvoiceWorker.Service.Concrete;
using Xero.InvoiceWorker.Service.Interface;
using Xunit;
using FluentAssertions;
using System;
using System.Net.Http;
using Xero.InvoiceWorker.Model;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace Xero.InvoiceWorker.UnitTest
{
    public class InvoiceWorkerServiceTest
    {
        private readonly Mock<IPdfGenerateService> _pdfService;
        private readonly IInvoiceWorkerService _service;
        private readonly Mock<ILogger> _logger;
        private readonly EventFeed _feed;
        public InvoiceWorkerServiceTest()
        {
            _pdfService = new Mock<IPdfGenerateService>();
            _logger = new Mock<ILogger>();
            _service = new InvoiceWorkerService(_pdfService.Object, _logger.Object);
            using (StreamReader r = new StreamReader("TestPayload/EventFeed.json"))
            {
                 _feed = JsonConvert.DeserializeObject<EventFeed>(r.ReadToEnd());
            }
        }

        [Fact]
        public async Task Should_Throw_Argument_Exception_For_Invalid_Endpoint()
        {
            //Arrange
            var endpoint = "invalidendpoint";

            //Action
            Func<Task> func = () => _service.Subscribe(endpoint);

            //Assert
            await func.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Should_Throw_Http_Exception_For_Unresponsive_Server()
        {
            //Arrange

            var endpoint = "http://iambroken/api";

            //Action
            Func<Task> func = () => _service.Subscribe(endpoint);

            //Assert
            await func.Should().ThrowAsync<HttpRequestException>();
        }

        [Fact]
        public async Task Should_Process_Create_Invoice_Feed()
        {
            //Arrange

            var feed = _feed;
            var invoiceDirectory = "somewhereInovice";
            var templateRootPath = "somewhereRootPath";

            //Action
            await _service.ProcessEventFeedCollection(feed, invoiceDirectory, templateRootPath);

            //Assert
            _pdfService.Verify(s => s.CreatePdfInvoice(invoiceDirectory, templateRootPath, feed.Items.First()), Times.Exactly(1));
        }

        [Fact]
        public async Task Should_Process_Delete_Invoice_Feed()
        {
            //Arrange

            var feed = _feed;
            var invoiceDirectory = "somewhereInovice";
            var templateRootPath = "somewhereRootPath";

            //Action
            await _service.ProcessEventFeedCollection(feed, invoiceDirectory, templateRootPath);

            //Assert
            _pdfService.Verify(s => s.DeletePdfInvoice(invoiceDirectory, feed.Items[1]), Times.Exactly(1));
        }

        [Fact]
        public async Task Should_Process_Update_Invoice_Feed()
        {
            //Arrange

            var feed = _feed;
            var invoiceDirectory = "somewhereInovice";
            var templateRootPath = "somewhereRootPath";

            //Action
            await _service.ProcessEventFeedCollection(feed, invoiceDirectory, templateRootPath);

            //Assert
            _pdfService.Verify(s => s.UpdatePdfInvoice(invoiceDirectory, templateRootPath, feed.Items[2]), Times.Exactly(1));
        }
    }
}
