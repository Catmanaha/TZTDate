namespace TZTDate.Infrastructure.Data.DateChat.PrivateChat.Commands;

using MediatR;
using TZTDate.Core.Data.DateChat.Entities;


public class GetCommand : IRequest<PrivateChat>
{
    public string CurrentUserId { get; set; }
    public string CompanionUserId { get; set; }
}