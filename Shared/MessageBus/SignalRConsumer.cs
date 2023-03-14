using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SignalRLib.Hubs;

namespace MessageBus
{
    public class SignalRConsumer : IConsumer<People>
    {
        IHubContext<PeopleHub> _hubContext;

        public SignalRConsumer(IHubContext<PeopleHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Consume(ConsumeContext<People> context)
        {
            var jsonMessage = JsonConvert.SerializeObject(context.Message);
            _hubContext.Clients.All.SendAsync("Send", jsonMessage);
            Console.WriteLine($"Created message: {jsonMessage}");
        }
    }
}