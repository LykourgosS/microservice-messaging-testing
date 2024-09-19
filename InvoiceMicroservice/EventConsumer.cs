using MassTransit;
using MessageContracts;
using System;
using System.Threading.Tasks;

namespace InvoiceMicroservice
{
    public class EventConsumer : IConsumer<IInvoiceToCreate>
    {
        public async Task Consume(ConsumeContext<IInvoiceToCreate> context)
        {
            var newInvoiceNumber = new Random().Next(10000, 99999);
            Console.WriteLine($"Creating invoice {newInvoiceNumber} for customer:{context.Message.CustomerNumber}");
            context.Message.InvoiceItems.ForEach(item => { Console.WriteLine(item.ToString()); });

            await Publish(context, newInvoiceNumber);
        }

        private async Task Publish(ConsumeContext<IInvoiceToCreate> context, int newInvoiceNumber)
        {
            await context.Publish<IInvoiceCreated>(new
            {
                InvoiceNumber = newInvoiceNumber,
                InvoiceData = new
                {
                    context.Message.CustomerNumber,
                    context.Message.InvoiceItems
                },
            });
        }
    }
}
