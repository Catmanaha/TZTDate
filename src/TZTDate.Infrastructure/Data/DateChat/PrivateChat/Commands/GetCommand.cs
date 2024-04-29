namespace TZTDate.Infrastructure.Data.DateChat.PrivateChat.Commands;

using MediatR;
using TZTDate.Core.Data.DateChat.Entities;


public class GetCommand : IRequest<PrivateChat>
{
    public int CurrentUserId { get; set; }
    public int CompanionUserId { get; set; }
}