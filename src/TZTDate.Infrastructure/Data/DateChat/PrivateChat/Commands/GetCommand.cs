namespace TZTDate.Infrastructure.Data.DateChat.PrivateChat.Commands;

using MediatR;
using TZTDate.Core.Data.DateUser.Chat;


public class GetCommand : IRequest<PrivateChat>
{
    public string CurrentUserId { get; set; }
    public string CompanionUserId { get; set; }
}