namespace MessageContracts
{
    public interface IInvoiceCreated
    {
        int InvoiceNumber { get; }
        IInvoiceToCreate InvoiceData { get; }
    }
}
