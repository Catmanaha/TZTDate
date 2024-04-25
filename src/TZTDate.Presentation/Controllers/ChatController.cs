namespace TZTDate.Presentation.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using TZTDate.Core.Data.DateUser;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Identity;
using TZTDate.Infrastructure.Data.ChatHub;
using TZTDate.Core.Data.DateChat.Entities;
using TZTDate.Core.Data.DateChat.ViewModels;
using TZTDate.Infrastructure.Data.DateChat.PrivateChat.Commands;
using System.Security.Authentication;

public class ChatController : Controller
{
    private readonly UserManager<User> userManager;
    private readonly IHubContext<ChatHub> hubContext;
    private readonly ISender sender;
    public ChatController(UserManager<User> userManager, IHubContext<ChatHub> hubContext, ISender sender)
    {
        this.hubContext = hubContext;
        this.sender = sender;
        this.userManager = userManager;
    }

    [HttpPost]
    public async Task<ActionResult> PrivateChat(string companionId)
    {
        var user = await userManager.GetUserAsync(User) ?? throw new AuthenticationException();

        var privateChat = await this.sender.Send<PrivateChat>(new GetCommand
        {
            CompanionUserId = companionId,
            CurrentUserId = user.Id
        });

        if (privateChat == null)
        {
            var newPrivateChatHashName = user.Id + companionId;
            var newPrivate = new PrivateChat
            {
                PrivateChatHashName = newPrivateChatHashName,
                Messages = new List<Message>()
            };
            await this.sender.Send(new AddCommand
            {
                NewPrivateChatHashName = newPrivateChatHashName,
            });
            return View(model: new CompanionsViewModel
            {
                CurrentUser = user,
                PrivateChat = newPrivate
            });
        }
        return View(model: new CompanionsViewModel
        {
            CurrentUser = user,
            PrivateChat = privateChat,
        });
    }
}