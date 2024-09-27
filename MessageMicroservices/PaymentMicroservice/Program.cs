using MessageContracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentMicroservice
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await StartService();
        }

        private static async Task StartService()
        {
            var busControl = BusControlCreator.CreateUsingRabbitMq<InvoiceCreatedConsumer>("payment-service");
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            Console.WriteLine("Payment Microservice Now Listening");

            try
            {
                while (true)
                {
                    //sit in while loop listening for messages
                    await Task.Delay(100);
                }
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}
