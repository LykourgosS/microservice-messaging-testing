using MassTransit;
using MessageContracts;
using System;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceMicroservice
{
    public class EventConsumer : IConsumer<IInvoiceToCreate>
    {
        public async Task Consume(ConsumeContext<IInvoiceToCreate> context)
        {
            var newInvoiceNumber = new Random().Next(10000, 99999);

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("----------------------------------------------------");
            stringBuilder.AppendLine($"Creating invoice {newInvoiceNumber} for customer:{context.Message.CustomerNumber} with items:");
            stringBuilder.AppendLine("----------------------------------------------------");
            context.Message.InvoiceItems.ForEach(item => { stringBuilder.AppendLine(item.ToString()); });

            Console.Write(stringBuilder.ToString());
            

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
