using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebApp.Hubs;

[Authorize]
public class ChatHub: Hub
{
    public async Task JoinChat(string url)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, url);
    }
    
    public Task LeaveChat(string url)
    {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, url);
    }

    public async Task SendMessage(string url, string message)
    {
        await Clients.Group(url).SendAsync("ReceiveMessage", message);
    }
}