using Microsoft.AspNetCore.SignalR;

namespace ICSServerApp.Additionals.Hubs;

public class DataHub : Hub
{
    public async Task SendData(string message)
    {
        await Clients.All.SendAsync("Receive", message);
    }
}