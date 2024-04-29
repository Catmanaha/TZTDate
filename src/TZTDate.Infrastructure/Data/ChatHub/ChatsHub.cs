namespace TZTDate.Infrastructure.Data.ChatHub;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TZTDate.Core.Data.DateChat.Entities;
using TZTDate.Core.Data.DateUser;

public class ChatHub : Hub
{
    private readonly TZTDateDbContext tZTDateDbContext;

    public ChatHub(TZTDateDbContext tZTDateDbContext)
    {
        this.tZTDateDbContext = tZTDateDbContext;
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
        var privateChat = await tZTDateDbContext.PrivateChats
            .FirstOrDefaultAsync(pc => pc.PrivateChatHashName == groupName);

        await tZTDateDbContext.Message.AddAsync(new Message {
            Owner = user,
            Content = message,
            PrivateChatId = privateChat?.Id ?? throw new ArgumentNullException()
        });
        
        await tZTDateDbContext.SaveChangesAsync();

        await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);
    }

    // public async Task SendToUser(string user, string receiverConnectionId, string message)
    // {
    //     await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", user, message);
    // }

    //public async Task SendMessage(string user, string message)
    //{
    //    await Clients.All.SendAsync("ReceiveMessage", user, message);
    //}
    public string GetConnectionId() => Context.ConnectionId;
}