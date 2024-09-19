using System.Text;

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
        public bool IsHazardousMaterial { get; set; }

        public override string? ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"With items: Price:{Price}, Desc:{Description}");
            builder.AppendLine($"Actual distance in miles:{ActualMileage}, Base Rate:{BaseRate}");
            builder.AppendLine($"Oversized:{IsOversized}, Refrigerated:{IsRefrigerated}, Haz Mat:{IsHazardousMaterial}");
            return builder.ToString();

        }
    }
}
