using MassTransit;
using System;

namespace MessageContracts
{
    public static class BusControlCreator
    {
        public static IBusControl CreateUsingRabbitMq<TConsumer>(string serviceName)
            where TConsumer : class, IConsumer, new()
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost");
                cfg.ReceiveEndpoint(serviceName, endpoint =>
                {
                    endpoint.UseInMemoryOutbox();
                    endpoint.Consumer<TConsumer>(c =>
                        c.UseMessageRetry(m => m.Interval(5, new TimeSpan(0, 0, 10)))
                    );
                });
            });
        }
    }
}
