using MediatR;
using Microsoft.EntityFrameworkCore;
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
        var currentUser = await tZTDateDbContext.Users.FirstOrDefaultAsync(user => user.Id == request.currentUserId) ?? throw new ArgumentNullException();
        var userToAction = await tZTDateDbContext.Users.FirstOrDefaultAsync(user => user.Id == request.userToActionId) ?? throw new ArgumentNullException();
        if (currentUser.FollowedId is not null && currentUser.FollowedId.Contains(userToAction.Id))
        {
            currentUser.FollowedId.Remove(userToAction.Id);
            userToAction.FollowersId?.Remove(currentUser.Id);
            await tZTDateDbContext.SaveChangesAsync();
            return;
        }
        currentUser.FollowedId = currentUser.FollowedId ?? new List<string>();
        userToAction.FollowersId = currentUser.FollowersId ?? new List<string>();
        currentUser.FollowedId.Add(userToAction.Id);
        userToAction.FollowersId.Add(currentUser.Id);
        await tZTDateDbContext.SaveChangesAsync();
        return;
    }
}
