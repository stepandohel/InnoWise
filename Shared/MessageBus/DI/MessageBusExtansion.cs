using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace MessageBus.DI
{
    public static class MessageBusExtansion
    {
        public static IServiceCollection AddMessageBus<T>(this IServiceCollection services, bool isDevelopment, string receiveEndpoint, string routingKey)
            where T : class, IConsumer
        {
            if (isDevelopment)
            {
                services.AddMassTransit(x =>
                {
                    x.AddConsumer<T>();
                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Publish<People>(x => x.ExchangeType = ExchangeType.Direct);
                        cfg.ReceiveEndpoint(receiveEndpoint, e =>
                        {
                            e.ConfigureConsumeTopology = false;
                            e.Bind<People>(x =>
                            {
                                x.ExchangeType = ExchangeType.Direct;
                                x.RoutingKey = routingKey;
                            });
                            e.ConfigureConsumer<T>(context);
                        });
                    });
                }
                );
            }
            else
            {
                services.AddMassTransit(x =>
                {
                    x.AddConsumer<T>();
                    x.UsingAzureServiceBus((context, cfg) =>
                    {
                        //cfg.Message<People>(x =>
                        //x.SetEntityName("user-created-event"
                        //));
                        cfg.Host("Endpoint=sb://dogel-stepan.servicebus.windows.net/;SharedAccessKeyName=getstarted;SharedAccessKey=+lM/2hSjqtXMqDxAnG2n8WPoC9vQYI/xT+ASbIqBm2w=");
                        cfg.ReceiveEndpoint(receiveEndpoint, e =>
                        {
                            e.ConfigureConsumer<T>(context);
                        });
                    });
                });
            }
                return services;
            }
        }
    }