using MassTransit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InvoiceMicroservice
{
    internal class Program
    {
        static async void Main(string[] args)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost");
                cfg.ReceiveEndpoint("invoice-service", endpoint =>
                {
                    endpoint.UseInMemoryOutbox();
                    endpoint.Consumer<EventConsumer>(c =>
                        c.UseMessageRetry(m => m.Interval(5, new TimeSpan(0, 0, 10)))
                    );
                });
            });
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
