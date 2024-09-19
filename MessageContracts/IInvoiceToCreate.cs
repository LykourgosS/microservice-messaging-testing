using System.Collections.Generic;

namespace MessageContracts
{
    public interface IInvoiceToCreate
    {
        int CustomerNumber { get; set; }
        List<InvoiceItems> InvoiceItems { get; set; }
    }
}
