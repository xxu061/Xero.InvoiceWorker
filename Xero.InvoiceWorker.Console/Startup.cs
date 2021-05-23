using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Xero.InvoiceWorker.App;
using Xero.InvoiceWorker.App.Concrete;
using Xero.InvoiceWorker.App.Interface;
using Xero.InvoiceWorker.Service.Concrete;
using Xero.InvoiceWorker.Service.Interface;

namespace Xero.InvoiceWorker.Console
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IInvoiceWorkerApp, InvoiceWorkerApp>();
            services.AddScoped<IInvoiceWorkerService, InvoiceWorkerService>();
            services.AddScoped<IPdfGenerateService, PdfGenerateService>();
            services.AddSingleton<IAppConfiguration, AppConfiguration>();
            services.AddSingleton(typeof(ILogger), typeof(Logger<Startup>));
        }
    }
}
