﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddSingleton(Configuration);
            services.AddSingleton<IInvoiceWorkerApp, InvoiceWorkerApp>();
            services.AddScoped<IInvoiceWorkerService, InvoiceWorkerService>();
            services.AddSingleton<IAppConfiguration, AppConfiguration>();
        }
    }
}