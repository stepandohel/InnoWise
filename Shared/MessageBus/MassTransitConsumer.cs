using MassTransit;
using Newtonsoft.Json;

namespace MessageBus
{
    public class MassTransitConsumer : IConsumer<People>
    {
        public async Task Consume(ConsumeContext<People> context)
        {
            var jsonMessage = JsonConvert.SerializeObject(context.Message);
            Console.WriteLine($"Created message: {jsonMessage}");
        }
    }
}
