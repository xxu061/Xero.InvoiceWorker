using System.Threading.Tasks;

namespace Xero.InvoiceWorker.App
{
    public interface IInvoiceWorkerApp
    {
        Task Subscribe(string endpoint, string invoiceDirectory);
    }
}