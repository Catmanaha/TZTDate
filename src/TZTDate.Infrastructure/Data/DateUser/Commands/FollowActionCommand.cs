using MediatR;
using TZTDate.Core.Data.DateUser;

namespace TZTDate.Infrastructure.Data.DateUser.Commands;

public class FollowActionCommand : IRequest
{
    public string currentUserId { get; set; }
    public string userToActionId { get; set; }
}