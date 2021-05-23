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

namespace Xero.InvoiceWorker.UnitTest
{
    public class InvoiceWorkerServiceTest
    {
        Mock<IPdfGenerateService> _pdfService;
        IInvoiceWorkerService _service;
        public InvoiceWorkerServiceTest()
        {
            _pdfService = new Mock<IPdfGenerateService>();
            _service = new InvoiceWorkerService(_pdfService.Object);
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
    }
}
