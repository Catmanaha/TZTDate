namespace TZTDate.Presentation.Controllers;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TZTDate.Core.Data.DateUser.Chat;
using TZTDate.Core.Data.DateUser;
using Microsoft.AspNetCore.SignalR;
using TZTDate.Infrastructure.Data.ChatHub;
using MediatR;
using TZTDate.Infrastructure.Data.DateChat.PrivateChat.Commands;

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
        var user = await userManager.GetUserAsync(User);

        var privateChat = await this.sender.Send<PrivateChat>(new GetCommand
        {
            CompanionUserId = companionId,
            CurrentUserId = user.Id
        });

        if (privateChat == null)
        {
            var newPrivateChatHashName = user.Id + companionId;
            await this.sender.Send(new AddCommand
            {
                NewPrivateChatHashName = newPrivateChatHashName,
            });
            return View(model: new CompanionsViewModel
            {
                CurrentUser = user,
                PrivateChatHashName = newPrivateChatHashName
            });
        }
        return View(model: new CompanionsViewModel
        {
            CurrentUser = user,
            PrivateChatHashName = privateChat.PrivateChatHashName
        });
    }
}
