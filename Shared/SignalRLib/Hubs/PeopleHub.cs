using Microsoft.AspNetCore.SignalR;

namespace SignalRLib.Hubs
{
    public class PeopleHub : Hub
    {
        public async Task Send(string message)
        {
            await this.Clients.All.SendAsync("Send", message);
        }
    }
}