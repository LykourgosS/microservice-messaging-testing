using MassTransit;
using MessageContracts;
using System;
using System.Threading.Tasks;

namespace PaymentMicroservice
{
    internal class InvoiceCreatedConsumer : IConsumer<IInvoiceCreated>
    {
        public async Task Consume(ConsumeContext<IInvoiceCreated> context)
        {
            await Task.Run(() =>
                Console.WriteLine($"Received message for invoice number: {context.Message.InvoiceNumber}")
            );
        }
    }
}
