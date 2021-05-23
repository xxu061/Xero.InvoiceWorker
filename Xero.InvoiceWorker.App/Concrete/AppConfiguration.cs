using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Xero.InvoiceWorker.App.Interface;

namespace Xero.InvoiceWorker.App.Concrete
{
    public class AppConfiguration: IAppConfiguration
    {
        private readonly IConfiguration _configuration;

        public AppConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string InvoiceApiEndpoint => string.Format(_configuration["eventFeedInvoiceUrl"], PageSize, AfterEventId);

        public int PageSize => int.Parse(_configuration["pageSize"]);

        public int AfterEventId => int.Parse(_configuration["afterEventId"]);

        public int MaxPageSize => int.Parse(_configuration["maxPageSize"]);

        public string TemplateRootPath => _configuration["templateRootPath"];
    }
}
