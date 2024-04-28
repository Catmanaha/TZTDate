using MediatR;
using Microsoft.EntityFrameworkCore;
using TZTDate.Core.Data.DateUser;
using TZTDate.Infrastructure.Data.DateUser.Commands;

namespace TZTDate.Infrastructure.Data.DateUser.Handlers;

public class FollowActionHandler : IRequestHandler<FollowActionCommand>
{
    private readonly TZTDateDbContext tZTDateDbContext;

    public FollowActionHandler(TZTDateDbContext tZTDateDbContext)
    {
        this.tZTDateDbContext = tZTDateDbContext;
    }

    public async Task Handle(FollowActionCommand request, CancellationToken cancellationToken)
    {
        if (request.currentUserId == request.userToActionId)
        {
            throw new InvalidOperationException("User cannot follow themselves.");
        }

        var currentUser = await tZTDateDbContext.Users.Include(u => u.Followed).FirstOrDefaultAsync(user => user.Id == request.currentUserId);
        var userToAction = await tZTDateDbContext.Users.Include(u => u.Followers).FirstOrDefaultAsync(user => user.Id == request.userToActionId);

        if (currentUser == null || userToAction == null)
        {
            throw new ArgumentException("User not found.");
        }

        var isFollowing = currentUser.Followed.Contains(userToAction);

        if (isFollowing)
        {
            currentUser.Followed.Remove(userToAction);
            userToAction.Followers.Remove(currentUser);
        }
        else
        {
            currentUser.Followed.Add(userToAction);
            userToAction.Followers.Add(currentUser);
        }

        await tZTDateDbContext.SaveChangesAsync(cancellationToken);
    }
}
