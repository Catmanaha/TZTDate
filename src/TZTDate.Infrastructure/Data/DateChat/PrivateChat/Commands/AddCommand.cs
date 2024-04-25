using MediatR;

namespace TZTDate.Infrastructure.Data.DateChat.PrivateChat.Commands;

public class AddCommand : IRequest
{
    public string NewPrivateChatHashName { get; set; }
}