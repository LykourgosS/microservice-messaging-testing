using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageContracts
{
    public interface IInvoiceToCreate
    {
        int CustomerNumber { get; set; }
        List<InvoiceItems> InvoiceItems { get; set; }
    }
}
