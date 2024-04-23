using MediatR;

namespace TZTDate.Infrastructure.Data.DateUser.Commands;

public class FollowActionCommand : IRequest
{
    public int currentUserId { get; set; }
    public int userToActionId { get; set; }
}