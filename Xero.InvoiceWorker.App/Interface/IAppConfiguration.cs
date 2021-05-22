using System;
using System.Collections.Generic;
using System.Text;

namespace Xero.InvoiceWorker.App.Interface
{
    public interface IAppConfiguration
    {
        string InvoiceApiEndpoint { get; }
        int PageSize { get; }
        int AfterEventId { get; }
        int MaxPageSize { get; }
    }
}
