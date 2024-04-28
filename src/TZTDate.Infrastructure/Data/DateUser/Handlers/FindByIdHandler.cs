using MediatR;
using Microsoft.EntityFrameworkCore;
using TZTDate.Core.Data.DateUser;
using TZTDate.Infrastructure.Data.DateUser.Commands;

namespace TZTDate.Infrastructure.Data.DateUser.Handlers
{
    public class FindByIdHandler : IRequestHandler<FindByIdCommand, User>
    {
        private readonly TZTDateDbContext context;
        public FindByIdHandler(TZTDateDbContext context)
        {
            this.context = context;

        }

        public async Task<User> Handle(FindByIdCommand request, CancellationToken cancellationToken)
        {
            if (request.Id < 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(request.Id)} cannot be negative");
            }

            var user = await context.Users
                            .Include(u => u.Address)
                            .Include(u => u.Followers)
                            .Include(u => u.Followed)
                            .Include(u => u.UserRoles)
                            .FirstOrDefaultAsync(u => u.Id == request.Id);

            if (user is null)
            {
                throw new NullReferenceException("User not found");
            }

            return user;
        }
    }
}