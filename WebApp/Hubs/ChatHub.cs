using App.Contracts.BLL;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebApp.Hubs;

public class ChatHub: Hub
{
    private readonly IAppBLL _uow;

    public ChatHub(IAppBLL uow)
    {
        _uow = uow;
    }
    
    public async Task JoinChat(string url)
    {
        var urlId = await GetOrCreateUrlId(url);
        var messages = await _uow.MessageService.GetPreviousMessages(urlId);

        await Groups.AddToGroupAsync(Context.ConnectionId, urlId.ToString());
        await Clients.Caller.SendAsync("ReceiveGroupId", urlId);
        await Clients.Caller.SendAsync("ReceiveMessages", messages);
    }
    
    public Task LeaveChat(Guid urlId)
    {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, urlId.ToString());
    }

    [Authorize]
    public async Task SendMessage(Guid urlId, string message)
    {
        var userId = Context.User!.GetUserId();
        var username = Context.User!.GetUsername();
        var addedMessage = await _uow.MessageService.Add(message, urlId, userId, username);
        await _uow.SaveChangesAsync();
        
        await Clients.Group(urlId.ToString()).SendAsync("ReceiveMessage", addedMessage);
    }
    
    private async Task<Guid> GetOrCreateUrlId(string url)
    {
        var urlId = await _uow.UrlService.GetOrCreateUrlId(url);
        await _uow.SaveChangesAsync();

        return urlId;
    }
}