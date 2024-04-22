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
        var currentUser = await tZTDateDbContext.Users.Include(u => u.Followed).FirstOrDefaultAsync(user => user.Id == request.currentUserId) ?? throw new ArgumentNullException();
        var userToAction = await tZTDateDbContext.Users.Include(u => u.Followers).FirstOrDefaultAsync(user => user.Id == request.userToActionId) ?? throw new ArgumentNullException();

        var isFollowing = await tZTDateDbContext.UserFollows
            .AnyAsync(uf => uf.FollowerId == currentUser.Id && uf.FollowedId == userToAction.Id);

        if (isFollowing)
        {
            var userFollow = await tZTDateDbContext.UserFollows
                .FirstOrDefaultAsync(uf => uf.FollowerId == currentUser.Id && uf.FollowedId == userToAction.Id);

            if (userFollow != null)
            {
                tZTDateDbContext.UserFollows.Remove(userFollow);
            }
        }
        else
        {
            tZTDateDbContext.UserFollows.Add(new UserFollow
            {
                FollowerId = currentUser.Id,
                FollowedId = userToAction.Id
            });
        }

        await tZTDateDbContext.SaveChangesAsync(cancellationToken);
    }
}
