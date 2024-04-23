using MediatR;
using Microsoft.EntityFrameworkCore;
using TZTDate.Core.Data.DateUser;
using TZTDate.Infrastructure.Data.DateUser.Commands;

namespace TZTDate.Infrastructure.Data.DateUser.Handlers;

public class GetUserRolesHandler : IRequestHandler<GetUserRolesCommand, IEnumerable<Role>>
{
    private readonly TZTDateDbContext context;
    public GetUserRolesHandler(TZTDateDbContext context)
    {
        this.context = context;

    }

    public async Task<IEnumerable<Role>> Handle(GetUserRolesCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
        .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
        .FirstOrDefaultAsync(u => u.Id == request.UserId);

        if (user == null)
        {
            throw new NullReferenceException("User not found");
        }

        var roles = user.UserRoles.Select(ur => ur.Role).ToList();
        return roles;
    }
}
