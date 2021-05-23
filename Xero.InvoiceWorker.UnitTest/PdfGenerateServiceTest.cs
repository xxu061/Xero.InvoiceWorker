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
    public class PdfGenerateServiceTest: IDisposable
    {
        private readonly IPdfGenerateService _service;
        private readonly Mock<ILogger> _logger;
        private readonly EventFeed _feed;
        private readonly string _invoiceDirectory = "TestPayloadOutput/";
        private readonly string _templateRootPath = "Templates/";
        public PdfGenerateServiceTest()
        {
            _logger = new Mock<ILogger>();
            _service = new PdfGenerateService(_logger.Object);
            using (StreamReader r = new StreamReader("TestPayload/EventFeed.json"))
            {
                _feed = JsonConvert.DeserializeObject<EventFeed>(r.ReadToEnd());
            }
        }

        [Fact]
        public async Task Should_Throw_Argument_Exception_For_Invalid_File_Directory()
        {
            //Arrange
            string invoiceDirectory = string.Empty;
            string templateRootPath = string.Empty;

            //Action
            Func<Task> func = () => _service.CreatePdfInvoice(invoiceDirectory, templateRootPath, _feed.Items[0]);

            //Assert
            await func.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Should_Throw_Argument_Exception_For_Invalid_Model()
        {
            //Arrange

            //Action
            Func<Task> func = () => _service.UpdatePdfInvoice(_invoiceDirectory, _templateRootPath, null);

            //Assert
            await func.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Should_Process_Create_And_Delete_Invoice_Feed()
        {
            //Arrange
            var feed = _feed;

            //Action
            Func<Task> createTask = () => _service.CreatePdfInvoice(_invoiceDirectory, _templateRootPath, _feed.Items[0]);
            Func<Task> deleteTask = () => _service.DeletePdfInvoice(_invoiceDirectory, _feed.Items[0]);

            //Assert
            await createTask.Should().NotThrowAsync();
            await deleteTask.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Should_Throw_File_Not_Found()
        {
            //Arrange

            //Action
            Func<Task> deleteTask = () => _service.DeletePdfInvoice(_invoiceDirectory, _feed.Items[1]);

            //Assert
            await deleteTask.Should().ThrowAsync<FileNotFoundException>();
        }

        public void Dispose()
        {

        }

        //[Fact]
        //public async Task Should_Process_Delete_Invoice_Feed()
        //{
        //    //Arrange

        //    var feed = _feed;
        //    var invoiceDirectory = "somewhereInovice";
        //    var templateRootPath = "somewhereRootPath";

        //    //Action
        //    await _service.ProcessEventFeedCollection(feed, invoiceDirectory, templateRootPath);

        //    //Assert
        //    _pdfService.Verify(s => s.DeletePdfInvoice(invoiceDirectory, feed.Items[1]), Times.Exactly(1));
        //}

        //[Fact]
        //public async Task Should_Process_Update_Invoice_Feed()
        //{
        //    //Arrange

        //    var feed = _feed;
        //    var invoiceDirectory = "somewhereInovice";
        //    var templateRootPath = "somewhereRootPath";

        //    //Action
        //    await _service.ProcessEventFeedCollection(feed, invoiceDirectory, templateRootPath);

        //    //Assert
        //    _pdfService.Verify(s => s.UpdatePdfInvoice(invoiceDirectory, templateRootPath, feed.Items[2]), Times.Exactly(1));
        //}
    }
}
