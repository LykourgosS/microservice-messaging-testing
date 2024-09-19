using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageContracts
{
    public class InvoiceItems
    {
        public string Description { get; set; }
        public double Price { get; set; }
        public double ActualMileage { get; set; }
        public double BaseRate { get; set; }
        public bool IsOversized { get; set; }
        public bool IsRefrigerated { get; set; }
        public bool IsHzardousMaterial { get; set; }
    }
}
