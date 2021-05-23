using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xero.InvoiceWorker.App;
using Xero.InvoiceWorker.Console;

namespace Xero.InvoiceWorker
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

            System.Console.WriteLine("Type in the service URL to connect to event feed:");
            var endpoint = System.Console.ReadLine();
            System.Console.WriteLine("Type in the file out put directory:");
            var fileDirectory = System.Console.ReadLine();

            // Get Service and call method
            var app = serviceProvider.GetService<IInvoiceWorkerApp>();
            await app.Subscribe(endpoint, fileDirectory);
        }
    }
}
