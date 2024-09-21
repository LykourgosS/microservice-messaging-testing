using MassTransit;
using MessageContracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InvoiceMicroservice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StartService();
        }

        private static async void StartService()
        {
            var busControl = BusControlCreator.CreateUsingRabbitMq<EventConsumer>("invoice-service");
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            Console.WriteLine("Invoice Microservice Now Listening");

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
