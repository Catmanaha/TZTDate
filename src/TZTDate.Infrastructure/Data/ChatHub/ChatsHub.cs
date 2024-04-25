namespace TZTDate.Infrastructure.Data.ChatHub;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using TZTDate.Core.Data.DateUser;

public class ChatHub : Hub
{
    public UserManager<User> usermanager;
    public ChatHub(UserManager<User> usermanager)
    {
        this.usermanager = usermanager;        
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    public Task JoinGroup(string groupName)
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task SendMessageToGroup(string user, string groupName, string message)
    {
        await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);
    }

    // public async Task SendToUser(string user, string receiverConnectionId, string message)
    // {
    //     await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", user, message);
    // }

    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
    public string GetConnectionId() => Context.ConnectionId;
}