using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xero.InvoiceWorker.Model;

namespace Xero.InvoiceWorker.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceWorkerController : ControllerBase
    {
        private readonly ILogger<InvoiceWorkerController> _logger;

        public InvoiceWorkerController(ILogger<InvoiceWorkerController> logger)
        {
            _logger = logger;
        }

        [Route("invoices/events")]
        [HttpGet]
        public async Task<EventFeed> Get(int pageSize, int afterEventId)
        {
            using (StreamReader r = new StreamReader("TestPayload/EventFeed.json"))
            {
                return await Task.FromResult(JsonConvert.DeserializeObject<EventFeed>(r.ReadToEnd()));
            }
        }
    }
}
