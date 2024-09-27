using MassTransit;
using MessageContracts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TestClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await StartService();
        }

        private static async Task StartService()
        {
            Console.WriteLine("Waiting while consumers initialize.");
            await Task.Delay(3000); //because the consumers need to start first

            var busControl = BusControlCreator.CreateUsingRabbitMq<InvoiceCreatedConsumer>("invoice-service-created");

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            var keyCount = 0;

            try
            {
                Console.WriteLine("Enter any key to send an invoice request or Q to quit.");

                while (Console.ReadKey(true).Key != ConsoleKey.Q)
                {
                    keyCount++;
                    await SendRequestForInvoiceCreation(busControl);
                    Console.WriteLine($"Enter any key to send an invoice request or Q to quit. {keyCount}");
                }
            }
            finally
            {
                await busControl.StopAsync();
            }
        }

        private static async Task SendRequestForInvoiceCreation(IPublishEndpoint publishEndpoint)
        {
            var random = new Random();
            await publishEndpoint.Publish<IInvoiceToCreate>(new {
                CustomerNumber = random.Next(1000, 9999),
                InvoiceItems = new List<InvoiceItems>()
                {
                    new() {
                        Description="Tables",
                        Price=Math.Round(random.NextDouble()*100,2),
                        ActualMileage = 40,
                        BaseRate=12.50,
                        IsHazardousMaterial=false,
                        IsOversized=true,
                        IsRefrigerated=false
                    },
                    new() {
                        Description="Chairs",
                        Price=Math.Round(random.NextDouble()*100,2),
                        ActualMileage = 40,
                        BaseRate=12.50,
                        IsHazardousMaterial=false,
                        IsOversized=false,
                        IsRefrigerated=false
                    }
                }
            });
        }
    }
}
