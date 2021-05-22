using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xero.InvoiceWorker.App;

namespace Xero.InvoiceWorker.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();

            Startup startup = new Startup();
            startup.ConfigureServices(services);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            IConfiguration _configuration = startup.Configuration;

            // Get Service and call method
            var app = serviceProvider.GetService<IInvoiceWorkerApp>();
            await app.Subscribe(args[0], args[1]);
        }
    }
}
