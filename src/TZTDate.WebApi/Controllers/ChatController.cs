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
using TZTDate.Infrastructure.Data.DateUser.Commands;
using TZTDate.WebApi.Filters;

[ApiController]
[Route("api/[controller]/[action]")]
[ServiceFilter(typeof(ValidationFilterAttribute))]
public class ChatController : ControllerBase
{
    private readonly IHubContext<ChatHub> hubContext;
    private readonly ISender sender;
    public ChatController(IHubContext<ChatHub> hubContext, ISender sender)
    {
        this.hubContext = hubContext;
        this.sender = sender;
    }

    [HttpPost]
    public async Task<ActionResult> PrivateChat(int companionId, int currentUserId)
    {
        var currentUser = await sender.Send(new FindByIdCommand { Id = currentUserId });
        var companionUser = await sender.Send(new FindByIdCommand { Id = companionId });
        var privateChat = await this.sender.Send<PrivateChat>(new GetCommand
        {
            CompanionUserId = companionId,
            CurrentUserId = currentUserId
        });

        if (privateChat == null)
        {
            var newPrivateChatHashName = currentUser.Email + companionUser.Email;
            var newPrivate = new PrivateChat
            {
                PrivateChatHashName = newPrivateChatHashName.ToString(),
                Messages = new List<Message>()
            };
            await this.sender.Send(new AddCommand
            {
                NewPrivateChatHashName = newPrivateChatHashName.ToString(),
            });
            return Ok(new CompanionsViewModel
            {
                CurrentUser = currentUser,
                PrivateChat = newPrivate
            });
        }
        return Ok(new CompanionsViewModel
        {
            CurrentUser = currentUser,
            PrivateChat = privateChat,
        });
    }
}