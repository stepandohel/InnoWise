using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace MessageBus.DI
{
    public static class MessageBusExtansion
    {
        public static IServiceCollection AddMessageBus(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<SignalRConsumer>();
                x.AddConsumer<MassTransitConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Publish<People>(x => x.ExchangeType = ExchangeType.Direct);
                    cfg.ReceiveEndpoint("order-created-event", e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.Bind<People>(x =>
                        {
                            x.ExchangeType = ExchangeType.Direct;
                            x.RoutingKey = "User";
                        });
                        //e.Consumer<MassTransitConsumer>();
                        e.ConfigureConsumer<SignalRConsumer>(context);
                    });

                    cfg.ReceiveEndpoint("order-created-event", e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.Bind<People>(x =>
                        {
                            x.ExchangeType = ExchangeType.Direct;
                            x.RoutingKey = "User";
                        });
                        e.Consumer<MassTransitConsumer>();
                    });
                });
            });

            return services;
        }

        public static IBusControl GetBusControl(string endpoint, string key)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.ReceiveEndpoint(endpoint, e =>
                {
                    e.ConfigureConsumeTopology = false;
                    e.Bind<People>(x =>
                    {
                        x.ExchangeType = ExchangeType.Direct;
                        x.RoutingKey = key;
                    });
                    e.Consumer<MassTransitConsumer>();
                });
            });

            return busControl;
        }
    }
}
